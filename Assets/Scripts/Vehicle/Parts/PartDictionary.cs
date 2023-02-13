using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VehicularVanguard.Vehicle
{
    public class PartDictionary : MonoBehaviour
    {
        public static Dictionary<string, PartData> Parts = new();
        public static string PartsDir = "parts/Structure";
        private void Awake() => InitList();
        public static void InitList()
        {
            var data = Resources.LoadAll<PartScriptable>(PartsDir).Select(_ => _.data).ToList();
            if (Parts.Count == data.Count) return;
            else Parts = new();
            data.ForEach(_ => Parts.Add(_.ID, _));
            print($"Added {Parts.Count()} parts");
        }
    }
}