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
namespace VehicularVanguard.Vehicle.Editor.Utils
{
    public class Resizer : MonoBehaviour
    {
        Camera _camera;
        public float ConstSize = .2f;
        void Awake() => _camera = Camera.main;
        void Update()
        {
            float mult = (transform.position - _camera.transform.position).magnitude;
            transform.localScale = new Vector3(ConstSize, ConstSize, ConstSize) * mult;
        }
    }
}