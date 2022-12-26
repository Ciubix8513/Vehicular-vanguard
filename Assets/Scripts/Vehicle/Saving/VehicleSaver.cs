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
                new());
            root.transform.parent.GetComponentsInChildren<Part>(true).Where(_ => !_.isRoot).ToList()
            .ForEach(c => v.Parts.Add(new(
                    c.GetHashCode(),
                    new(c.transform),
                    c.partData.ID,
                    c.parentPart.GetHashCode()
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
    }
}