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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VehicularVanguard.Vehicle.Saving
{
    [Serializable]
    public struct BindsStruct
    {
        public string Name;
        public KeyCode Key;
        public int Action;

        public BindsStruct(string item1, Tuple<KeyCode , int> item2)
        {
            Name = item1;
            Key = item2.Item1;
            Action = item2.Item2;
        }
    }
    [Serializable]
    public struct PartSaveStruct
    {

        public int SaveID;
        public TransformData Transform;
        public string ID;
        public int AttachedPartID;
        public Attachments OccupiedFaces;
        public List<BindsStruct> Binds;

        public PartSaveStruct(int saveID, TransformData transform, string iD, int attachedPartID, Attachments occupiedFaces, Dictionary<string, Tuple<KeyCode, int>> binds)
        {
            SaveID = saveID;
            Transform = transform;
            ID = iD;
            AttachedPartID = attachedPartID;
            OccupiedFaces = occupiedFaces;
            Binds = binds.Select(_ => new BindsStruct(_.Key,_.Value)).ToList();
        }
    }
}