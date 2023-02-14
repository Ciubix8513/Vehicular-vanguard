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

namespace VehicularVanguard.Vehicle.Parts
{
    public class Wheel : Part
    {
        [SerializeField]
        private Transform _wheelMesh;
        public WheelCollider WheelCollider;
        public bool HasMotor = false;
        public bool CanSteer = false;
        public float LeftMax = 45.0f;
        public float RightMax = -45.0f;
        public float TurningSpeed = .1f;
        private float _targetAngle = 0.0f;
        private void SteerLeft() => _targetAngle += LeftMax;
        private void SteerRight() => _targetAngle += RightMax;
        private void StopSteerLeft() => _targetAngle -= LeftMax;
        private void StopSteerRight() => _targetAngle -= RightMax;
        public float MaxTorque = 100.0f;
        private float _targetTorque = 0.0f;
        [SerializeField]
        private float Acceleration = .1f;
        public override void Activate()
        {
            _targetTorque = MaxTorque;
            WheelCollider.brakeTorque = 0.0f;
        }
        public override void DeActivate()
        {
            _targetTorque = 0;
            WheelCollider.motorTorque = 0;
            WheelCollider.brakeTorque = 20.0f;
        }
        public bool CanBreak = false;
        public float MaxBreakTorque = 100.0f;
        [SerializeField]
        private float _breakAcceleration = .1f;
        private float _targetBreak;
        public void StartBreak() => _targetBreak = MaxBreakTorque;
        public void StopBreak() => _targetBreak = 0.0f;
        public override List<ActionData> GetActions()
        {
            List<ActionData> _ = new();
            if (CanSteer)
            {
                _.Add(new(SteerLeft, "Steer left", KeyCode.A, 0));
                _.Add(new(SteerRight, "Steer right", KeyCode.D, 0));
                _.Add(new(StopSteerLeft, "Stop steer left", KeyCode.A, 1));
                _.Add(new(StopSteerRight, "Stop steer right", KeyCode.D, 1));
            }
            if (HasMotor)
            {
                _.Add(new(Activate, "Turn on motor", KeyCode.W, 0));
                _.Add(new(DeActivate, "Turn off motor", KeyCode.W, 1));
            }
            if (CanBreak)
            {
                _.Add(new(StartBreak, "Turn on breaks", KeyCode.S, 0));
                _.Add(new(StopBreak, "Turn off breaks", KeyCode.S, 1));
            }
            return _;
        }
        private void FixedUpdate()
        {
            WheelCollider.steerAngle = Mathf.Lerp(WheelCollider.steerAngle, _targetAngle, TurningSpeed);
            WheelCollider.motorTorque = Mathf.Lerp(WheelCollider.motorTorque, _targetTorque, Acceleration);
            WheelCollider.brakeTorque = Mathf.Lerp(WheelCollider.brakeTorque, _targetBreak, _breakAcceleration);
            WheelCollider.GetWorldPose(out var pos, out var rot);
            _wheelMesh.position = pos;
            _wheelMesh.rotation = rot;
        }
    }
}