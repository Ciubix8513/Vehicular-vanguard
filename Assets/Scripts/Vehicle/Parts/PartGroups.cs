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

namespace VehicularVanguard.Vehicle
{
    public class PartGroups : MonoBehaviour
    {
        public GroupWrapper DownGroup;
        public GroupWrapper UpGroup;
        public static PartGroups Instance;
        public GroupWrapper this[int i]
        {
            get
            {
                if (i == 0)
                    return DownGroup;
                else
                    return UpGroup;
            }
        }
        void Awake()
        {
            Instance = this;
            DownGroup = new();
            UpGroup = new();
        }
    }
}