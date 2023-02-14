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