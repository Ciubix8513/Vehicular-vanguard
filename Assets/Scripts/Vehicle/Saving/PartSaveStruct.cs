using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarGame.Vehicle.Saving
{
    [Serializable]
    public struct BindsStruct
    {
        public string Item1;
        public KeyCode Item2;
        public int Item3;

        public BindsStruct(string item1, Tuple<KeyCode , int> item2)
        {
            Item1 = item1;
            Item2 = item2.Item1;
            Item3 = item2.Item2;
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
        // public Dictionary<string, Tuple<KeyCode, int>> Binds;
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