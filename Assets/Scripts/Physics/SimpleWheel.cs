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

[RequireComponent(typeof(Rigidbody))]
public class SimpleWheel : MonoBehaviour
{
    public Vector3 WheelSize = Vector3.one;
    public float Torque = 0.0f;
    public float BreakTorque = 0.0f;
    //The axis of rotation
    public Vector3 RotationAxis = Vector3.up;
    public float MaxRotation = 10.0f;
    public float TargetRotation = 0.0f;
    public float RotationForce = 1.0f;
    private Rigidbody _rigidbody;
    void Awake() => _rigidbody = GetComponent<Rigidbody>();

    /// <summary>
    /// Calculates The space needed for a wheel to rote properly
    /// </summary>
    /// <returns></returns>
    public float CalculateWheelOffset()
    {
        float y = WheelSize.z / 2;
        float x = WheelSize.x / 2;
        float alpha = Mathf.Deg2Rad * MaxRotation;
        float b = Mathf.Sin(alpha) * y;
        float d = Mathf.Cos(alpha- Mathf.PI) * x;

        return b + d;
    }

    void FixedUpdate()
    {
        //Use force for motor and breaks
        //Use Impulse for rotation 
        // _rigidbody.AddTorque())
        // Quaternion.
    }
}