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
using UnityEngine;

namespace VehicularVanguard.Vehicle
{
    [Serializable]
    public struct Attachments
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool forward;
        public bool backward;
        public bool this[Vector3Int i]
        {
            get
            {
                if (i == Vector3Int.right)
                    return right;
                else if (i == Vector3Int.left)
                    return left;
                else if (i == Vector3Int.up)
                    return up;
                else if (i == Vector3Int.down)
                    return down;
                else if (i == Vector3Int.forward)
                    return forward;
                else
                    return backward;
            }
            set
            {
                if (i == Vector3Int.right)
                    right = value;
                else if (i == Vector3Int.left)
                    left = value;
                else if (i == Vector3Int.up)
                    up = value;
                else if (i == Vector3Int.down)
                    down = value;
                else if (i == Vector3Int.forward)
                    forward = value;
                else
                    backward = value;
            }
        }
    }
}