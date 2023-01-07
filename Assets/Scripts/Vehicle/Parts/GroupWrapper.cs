using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace CarGame.Vehicle
{
    public class GroupWrapper
    {
        public Dictionary<KeyCode, List<(Part.ActionDel, int)>> group;
        public GroupWrapper() => group = new();
        public void Add(KeyCode key, (Part.ActionDel, int) action)
        {
            if (!group.ContainsKey(key))
                group.Add(key, new());
            group[key].Add(action);
        }
        public void RemoveAllByInstanceId(int id) => group.Select(_ => _.Value)
            .ToList()
            .ForEach(_ => _.RemoveAll(_ => _.Item2 == id));
    }
}