//The GPLv3 License (GPLv3)
//
//Copyright (c) 2023 Ciubix8513
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VehicularVanguard.Vehicle
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