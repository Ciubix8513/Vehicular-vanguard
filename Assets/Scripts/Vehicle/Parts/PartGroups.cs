using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GroupWrapper
{
    public Dictionary<KeyCode,List<(Part.ActionDel,int)>> group;
    public GroupWrapper() => group = new();
    public void Add(KeyCode key,(Part.ActionDel, int) action)
    {
        if(!group.ContainsKey(key))
            group.Add(key,new());
        group[key].Add(action);
    }
    public void RemoveAllByInstanceId(int id) => group.Select(_ => _.Value)
        .ToList()
        .ForEach(_ => _.RemoveAll(_ => _.Item2 == id));
}

public class PartGroups : MonoBehaviour
{
    public static GroupWrapper DownGroup;
    public static GroupWrapper UpGroup;
    void Awake()
    {
        Debug.Log("Called awake");
        DownGroup = new();
        UpGroup = new();
    }
}
