using System.Collections.Generic;
using System.IO;
using System.Linq;
using CarGame.Player;
using CarGame.Vehicle.Editor;
using CarGame.Vehicle.Editor.UI;
using UnityEngine;

namespace CarGame.Vehicle.Saving
{
#nullable enable
    public class VehicleSaver
    {
        //Save a vehicle based on the root
        public static Vehicle SerializeVehicle(Part root)
        {
            Vehicle v = new(
                Application.version,
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
            return v;
        }
        public static HistoryVehicle SerializeHistoryVehicle(Part root)
        {
            Debug.Log("Serialized history vehicle in " + InputManager.editorMode + " mode");
            return new(
                SerializeVehicle(root),
                InputManager.editorMode,
                InputManager.editorMode == EditorMode.rotate ? GizmoRaycaster.RotatingObjectId : InputMenu.SelectedId);
        }

        public static void SaveVehicle(Vehicle v, string name)
        {
            var json = JsonUtility.ToJson(v, true);
            var Dir = Application.persistentDataPath + $"/Vehicles/";
            if (!Directory.Exists(Dir))
                Directory.CreateDirectory(Dir);
            File.WriteAllText(Dir + $"{name}.vehicle", json);
        }
        public static Vehicle LoadVehicle(string name)
        {
            var path = Application.persistentDataPath + $"/Vehicles/{name}.vehicle";
            if (!File.Exists(path))
            {
                Debug.LogError($"File {path} does not exist");
                return null;
            }
            var json = File.ReadAllText(path);
            var v = JsonUtility.FromJson<Vehicle>(json);
            if (v.Version != Application.version)
            {
                Debug.LogError("Cannot load a file created in a different version");
                return null;
            }
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
                PartGroups.DownGroup.RemoveAllByInstanceId(_.gameObject.GetInstanceID());
                PartGroups.UpGroup.RemoveAllByInstanceId(_.gameObject.GetInstanceID());
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
                var act = p.GetActions().ToDictionary(_ => _.Item2, _ => _.Item1);
                _.Binds.ForEach(_ =>
                {
                    //0 = Down, 1 = up
                    if (_.Item3 == 0) PartGroups.DownGroup.Add(_.Item2, (act[_.Item1], p.gameObject.GetInstanceID()));
                    else PartGroups.UpGroup.Add(_.Item2, (act[_.Item1], p.gameObject.GetInstanceID()));
                    p.binds.Add(_.Item1, new(_.Item2, _.Item3));
                });
            });
            return root;
        }
    }
}