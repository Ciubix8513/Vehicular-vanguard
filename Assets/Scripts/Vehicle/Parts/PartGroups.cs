using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartGroups : MonoBehaviour
{
    public static List<List<Part>> groups = new();
    private int numGroups = 1;
    private void Awake() 
    {
        for(int i = 0; i < numGroups; i++)
        groups.Add(new());
    }
    public static void AddToGroup(int group, Part part)
    {        
        if (group > groups.Count - 1)
        {
            group = groups.Count;
            groups.Add(new());
        }
        groups[group].Add(part);
    }
    public static void ProcessGroupDown(int group) => groups[group]?.ForEach(i => i.Activate());
    public static void ProcessGroupUp(int group) => groups[group]?.ForEach(i => i.DeActivate());

}
