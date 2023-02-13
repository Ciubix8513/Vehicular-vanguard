using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VehicularVanguard.Vehicle.Saving
{
    [Serializable]
    public struct BindsStruct
    {
        public string Name;
        public KeyCode Key;
        public int Action;

        public BindsStruct(string item1, Tuple<KeyCode , int> item2)
        {
            Name = item1;
            Key = item2.Item1;
            Action = item2.Item2;
        }
    }
    [Serializable]
    public struct PartSaveStruct
    {

        public int SaveID;
        public TransformData Transform;
        public string ID;
        public int AttachedPartID;
        public Attachments OccupiedFaces;
        public List<BindsStruct> Binds;

        public PartSaveStruct(int saveID, TransformData transform, string iD, int attachedPartID, Attachments occupiedFaces, Dictionary<string, Tuple<KeyCode, int>> binds)
        {
            SaveID = saveID;
            Transform = transform;
            ID = iD;
            AttachedPartID = attachedPartID;
            OccupiedFaces = occupiedFaces;
            Binds = binds.Select(_ => new BindsStruct(_.Key,_.Value)).ToList();
        }
    }
}