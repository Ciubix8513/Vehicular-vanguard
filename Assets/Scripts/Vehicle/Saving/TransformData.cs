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

namespace VehicularVanguard.Vehicle.Saving
{
    [System.Serializable]
    public struct TransformData
    {
        public Vector3 Position;
        public Vector3 RotationEuler;
        public Vector3 Scale;

        public TransformData(Transform t)
        {
            Position = t.localPosition;
            RotationEuler = t.localEulerAngles;
            Scale = t.localScale;
            //This is such a stupid fix for a stupid problem
            //TODO remove beyond editor demos
            if(Mathf.Abs(t.localPosition.y) < .004f)
            Position.y = 0;
        }
    }
    //An extension class and an extension function just to make my life a bit easier
    public static class TransformDataTransformExtension
    {
        public static void Load(this Transform t, TransformData d)
        {
            t.localPosition = d.Position;
            t.localEulerAngles = d.RotationEuler;
            t.localScale = d.Scale;
        }
    }
}