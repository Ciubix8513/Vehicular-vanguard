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
using System.Linq;
using VehicularVanguard.Vehicle;
using VehicularVanguard.Vehicle.Saving;
using UnityEngine;

namespace VehicularVanguard.Player
{
    public class PlayerVehicle : MonoBehaviour
    {
        public Part Root;
        private TransformData _initialPosition;
        void Awake() => _initialPosition = new TransformData(transform);

        public void ResetTransform()
        {
            transform.Load(_initialPosition);
            GetComponentsInChildren<Rigidbody>().ToList().ForEach(_ =>
            {
                _.velocity = Vector3.zero;
                _.angularVelocity = Vector3.zero;
            });
        }
    }
}