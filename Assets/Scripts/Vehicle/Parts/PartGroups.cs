using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PartGroups : MonoBehaviour
{

    public static Dictionary<KeyCode, List<Tuple<Part.ActionDel, int>>> DownGroup;
    public static Dictionary<KeyCode, List<Tuple<Part.ActionDel, int>>> UpGroup;

    private void Awake()
    {
        DownGroup = new();
        UpGroup = new();
    }

}
