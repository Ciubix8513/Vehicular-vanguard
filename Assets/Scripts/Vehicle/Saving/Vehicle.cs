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

namespace VehicularVanguard.Vehicle.Saving
{
    [System.Serializable]
    //Making it a class so there's less copying
    public class Vehicle
    {
        public Vehicle(Version version,
                       TransformData rootTransform,
                       string rootID,
                       int rootSaveID,
                       List<PartSaveStruct> parts,
                       Attachments rootOccupiedFaces)
        {
            Version = version;
            RootTransform = rootTransform;
            RootID = rootID;
            RootSaveID = rootSaveID;
            Parts = parts;
            RootOccupiedFaces = rootOccupiedFaces;
        }

        public Version Version;
        public TransformData RootTransform;
        public string RootID;
        public int RootSaveID;
        public Attachments RootOccupiedFaces;
        public List<PartSaveStruct> Parts;
        public byte[] PreviewImage;
        //While I can get these from file names, i'm gonna add them here just for some extra convenience
        public string CreationDate;
        public string Name;
        public string FileName;
    }
}