using UnityEngine;
using System;

namespace VehicularVanguard.Vehicle
{

    [Serializable]
    public class PartData
    {
        public string name;
        public string description;
        public GameObject prefab;
        public Sprite sprite;
        public string ID;
        public bool ExcludeFromBuildMenu;
    }

    [CreateAssetMenu(fileName = "Part", menuName = "SOs/Part", order = 0)]
    public class PartScriptable : ScriptableObject
    {
        public PartData data = new();
    }
}