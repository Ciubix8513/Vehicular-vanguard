using System.Collections.Generic;
using UnityEngine;
using System;

public class PartGroups : MonoBehaviour
{
    public static Dictionary<KeyCode, List<Tuple<Part.ActionDel, int>>> DownGroup = new();
    public static Dictionary<KeyCode, List<Tuple<Part.ActionDel, int>>> UpGroup = new();
}
