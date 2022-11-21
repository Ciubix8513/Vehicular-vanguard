using System;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : Part
{
    public WheelCollider WheelCollider;
    public bool HasMotor = false;
    public bool CanSteer = false;
    public float LeftMax = 45.0f;
    public float RightMax = -45.0f;
    public float TurningSpeed = 1.0f;
    private float _targetAngle = 0.0f;
    private void SteerLeft() => _targetAngle += LeftMax;
    private void SteerRight() => _targetAngle += RightMax;
    private void StopSteerLeft() => _targetAngle -= LeftMax;
    private void StopSteerRight() => _targetAngle -= RightMax;
    public float MaxTorque = 100.0f;
    private float _targetTorque = 0.0f;
    public override void Activate() => _targetTorque = MaxTorque;
    public override void DeActivate() => _targetTorque = 0;
    public override List<Tuple<ActionDel, string>> GetActions()
    {
        List<Tuple<ActionDel, string>> o = new();
        if (CanSteer)
        {
            o.Add(new(SteerLeft, "Steer left"));
            o.Add(new(StopSteerLeft, "Stop steer left"));
            o.Add(new(SteerRight, "Steer right"));
            o.Add(new(StopSteerRight, "Stop steer right"));
        }
        if (HasMotor)
        {
            o.Add(new(Activate, "Turn on motor"));
            o.Add(new(DeActivate, "Turn off motor"));
        }
        return o;
    }
    private void FixedUpdate() 
    {
        
    }
}
