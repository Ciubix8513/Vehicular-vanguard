using System;
using System.Collections.Generic;

namespace CarGame.Vehicle.Saving
{
    [System.Serializable]
    //Making it a class so there's less copying
    public class Vehicle
    {
        public Vehicle(Version version,
                       TransformData rootTransform,
                       string rootID,
                       int rootSaveID,
                       List<PartSaveStruct> parts,
                       Attachments rootOccupiedFaces)
        {
            Version = version;
            RootTransform = rootTransform;
            RootID = rootID;
            RootSaveID = rootSaveID;
            Parts = parts;
            RootOccupiedFaces = rootOccupiedFaces;
        }

        public Version Version;
        public TransformData RootTransform;
        public string RootID;
        public int RootSaveID;
        public Attachments RootOccupiedFaces;
        public List<PartSaveStruct> Parts;
        public byte[] PreviewImage;
        //While I can get these from file names, i'm gonna add them here just for some extra convenience
        public string CreationDate;
        public string Name;
    }
}