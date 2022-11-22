using System;
using System.Collections.Generic;
using UnityEngine;

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
    public void StartBreak()=>_targetBreak = MaxBreakTorque;
    public void StopBreak() => _targetBreak = 0.0f;
    public override List<Tuple<ActionDel, string>> GetActions()
    {
        List<Tuple<ActionDel, string>> o = new();
        if (CanSteer)
        {
            o.Add(new(SteerLeft, "Steer left"));
            o.Add(new(SteerRight, "Steer right"));
            o.Add(new(StopSteerLeft, "Stop steer left"));
            o.Add(new(StopSteerRight, "Stop steer right"));
        }
        if (HasMotor)
        {
            o.Add(new(Activate, "Turn on motor"));
            o.Add(new(DeActivate, "Turn off motor"));
        }
        if(CanBreak)
        {
            o.Add(new(StartBreak,"Turn on breaks"));
            o.Add(new(StopBreak,"Turn off breaks"));
        }
        return o;
    }
    private void FixedUpdate()
    {
        WheelCollider.steerAngle = Mathf.Lerp(WheelCollider.steerAngle, _targetAngle, TurningSpeed);
        WheelCollider.motorTorque = Mathf.Lerp(WheelCollider.motorTorque, _targetTorque, Acceleration);
        WheelCollider.brakeTorque = Mathf.Lerp(WheelCollider.brakeTorque,_targetBreak,_breakAcceleration);
        Debug.Log($"Motor torque:{WheelCollider.motorTorque}");
        WheelCollider.GetWorldPose(out var pos, out var rot);
        _wheelMesh.position = pos;
        _wheelMesh.rotation = rot;
    }
}
