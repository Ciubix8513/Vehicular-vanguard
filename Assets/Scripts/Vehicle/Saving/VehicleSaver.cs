//The GPLv3 License (GPLv3)
//
//Copyright (c) 2023 Ciubix8513
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VehicularVanguard.Player;
using VehicularVanguard.Vehicle.Editor;
using VehicularVanguard.Vehicle.Editor.UI;
using UnityEngine;

namespace VehicularVanguard.Vehicle.Saving
{
#nullable enable
    public class VehicleSaver
    {
        //Save a vehicle based on the root
        public static Vehicle SerializeVehicle(Part root, bool saveImage = false, RenderTexture source = null)
        {
            Vehicle v = new(
                 new Version(Application.version),
                new(root.transform),
                root.partData.ID,
                root.GetHashCode(),
                new(),
                root.attachedParts);
            root.transform.parent.GetComponentsInChildren<Part>(true).Where(_ => !_.isRoot).ToList()
            .ForEach(
                c => v.Parts.Add(new(
                c.GetHashCode(),
                new(c.transform),
                c.partData.ID,
                c.parentPart.GetHashCode(),
                c.attachedParts,
                c.binds)));
            if (!saveImage)
                return v;
            if (source == null)
            {
                Debug.LogError("Image source must not be null");
                return v;
            }
            Texture2D tex = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
            RenderTexture.active = source;
            tex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
            tex.Apply();
            v.PreviewImage = tex.EncodeToPNG();
            return v;
        }
        public static HistoryVehicle SerializeHistoryVehicle(Part root) => new(
                SerializeVehicle(root),
                InputManager.editorMode,
                InputManager.editorMode == EditorMode.rotate ? GizmoRaycaster.RotatingObjectId : InputMenu.SelectedId);

        public static void SaveVehicle(Vehicle v, string name)
        {
            var json = JsonUtility.ToJson(v, true);
            var Dir = Application.persistentDataPath + $"/Vehicles/";
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
            File.WriteAllText(Dir + $"{name}.vehicle", json);
        }
        public static Vehicle LoadVehicle(string name, bool isPath = false)
        {
            var path = Application.persistentDataPath + $"/Vehicles/{name}.vehicle";
            if (isPath)
                path = name;
            if (!File.Exists(path))
            {
                Debug.LogError($"File {path} does not exist");
                return null;
            }
            var json = File.ReadAllText(path);
            var v = JsonUtility.FromJson<Vehicle>(json);
            v.FileName = path;
            return v;
        }
        public static (Part?, Part?) GenerateHistoryVehicle(HistoryVehicle v, Transform parent) =>
            (GenerateVehicle(v.Vehicle,
                             parent,
                             new List<int>() { v.SelectedPartId },
                             out List<Part> o), o?.Count() == 0 ? null : o?[0]);

        //Generates a vehicle from serialized data, returns the Part component of the root
        public static Part? GenerateVehicle(Vehicle v, Transform parent,
        List<int>? requestedIndices,
        out List<Part>? requestedParts)
        {
            requestedParts = null;
            if (v == null) return null;
            //Destroy all children
            parent.Cast<Transform>().ToList().ForEach(_ =>
            {
                //Remove the key binds
                PartGroups.Instance[0].RemoveAllByInstanceId(_.gameObject.GetInstanceID());
                PartGroups.Instance[1].RemoveAllByInstanceId(_.gameObject.GetInstanceID());
                Object.Destroy(_.gameObject);
            });
            var rootData = PartDictionary.Parts[v.RootID];
            var root = Object.Instantiate(
                rootData.prefab,
                parent,
                false).GetComponent<Part>();
            root.partData = rootData;
            root.transform.Load(v.RootTransform);
            //A dictionary for faster loading
            var lookUp = v.Parts.ToDictionary(_ => _.SaveID,
            _ => Object.Instantiate(
                PartDictionary.Parts[_.ID].prefab,
                parent,
                false).GetComponent<Part>());
            lookUp.Add(v.RootSaveID, root);
            requestedParts = requestedIndices?.Where(
                _ => lookUp.ContainsKey(_)).Select(_ => lookUp[_]).ToList();
            //Assign the part data
            v.Parts.ForEach(_ =>
            {
                var p = lookUp[_.SaveID];
                p.partData = PartDictionary.Parts[_.ID];
                p.transform.Load(_.Transform);
                p.attachedParts = _.OccupiedFaces;
                p.parentPart = lookUp[_.AttachedPartID];
                if (p.parentPart == null) return;
                (p.Joint = p.gameObject.AddComponent<FixedJoint>()).connectedBody = p.parentPart.GetComponent<Rigidbody>();
                var act = p.GetActions().ToDictionary(_ => _.Name, _ => _.Delegate);
                _.Binds.ForEach(_ =>
                {
                    PartGroups.Instance[_.Action].Add(_.Key, (act[_.Name], p.gameObject.GetInstanceID()));
                    p.binds.Add(_.Name, new(_.Key, _.Action));
                });
            });
            return root;
        }
    }
}