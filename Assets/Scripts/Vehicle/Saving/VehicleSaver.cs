using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace CarGame.Vehicle.Saving
{
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
            .ForEach(c => v.Parts.Add(new(
                    c.GetHashCode(),
                    new(c.transform),
                    c.partData.ID,
                    c.parentPart.GetHashCode(),
                    c.attachedParts,
                    c.binds
                )));
            return v;
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
        //Generates a vehicle from serialized data, returns the Part component of the root
        public static Part GenerateVehicle(Vehicle v, Transform parent)
        {
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
            var root = Object.Instantiate(rootData.prefab,
                                   parent,
                                   false).GetComponent<Part>();
            root.partData = rootData;
            root.transform.Load(v.RootTransform);
            //A dictionary for faster loading
            var lookUp = new Dictionary<int, Part> { { v.RootSaveID, root } };
            //Populate the look up
            v.Parts.ForEach(_ => lookUp.Add(_.SaveID,
            Object.Instantiate(PartDictionary.Parts[_.ID].prefab, parent, false).
            GetComponent<Part>()));
            v.Parts.ForEach(_ =>
            {
                var p = lookUp[_.SaveID];
                p.partData = PartDictionary.Parts[_.ID];
                p.transform.Load(_.Transform);
                p.attachedParts = _.OccupiedFaces;
                p.parentPart = lookUp[_.AttachedPartID];
                if (p.parentPart == null) return;
                var f = p.gameObject.AddComponent<FixedJoint>();
                f.connectedBody = p.parentPart.GetComponent<Rigidbody>();
                var act = p.GetActions().ToDictionary(_ => _.Item2, _ => _.Item1);
                _.Binds.ForEach(_ =>
                {
                    //0 = Down, 1 = up
                    if (_.Item3 == 0) PartGroups.DownGroup.Add(_.Item2, (act[_.Item1], p.gameObject.GetInstanceID()));
                    else PartGroups.UpGroup.Add(_.Item2, (act[_.Item1], p.gameObject.GetInstanceID()));
                    p.binds.Add(_.Item1,new(_.Item2,_.Item3));
                });
            });
            return root;
        }
    }
}