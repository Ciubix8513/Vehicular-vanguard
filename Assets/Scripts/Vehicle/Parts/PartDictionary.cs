using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartDictionary : MonoBehaviour
{
    public static Dictionary<string, PartData> Parts = new();
    public static string PartsDir = "parts/Structure";

    private void Awake() => InitList();
    public static void InitList()
    {
        var data = Resources.LoadAll<PartScriptable>(PartsDir).Select(_ => _.data).ToList();
        print($"trying to add {data.Count()} parts");
        data.ForEach(_ => Parts.Add(_.ID.ToLower(), _));
        print($"Added {Parts.Count()} parts");
    }
}
