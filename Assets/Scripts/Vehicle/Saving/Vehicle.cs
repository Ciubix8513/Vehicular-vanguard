using System.Collections.Generic;

namespace CarGame.Vehicle.Saving
{
    [System.Serializable]
    //Making it a class so there's less copying
    public class Vehicle
    {
        public Vehicle(string version, TransformData rootTransform, string rootID, int rootSaveID, List<PartSaveStruct> parts)
        {
            Version = version;
            RootTransform = rootTransform;
            RootID = rootID;
            RootSaveID = rootSaveID;
            Parts = parts;
        }

        public string Version;
        public TransformData RootTransform;
        public string RootID;
        public int RootSaveID;
        public List<PartSaveStruct> Parts;
    }
}