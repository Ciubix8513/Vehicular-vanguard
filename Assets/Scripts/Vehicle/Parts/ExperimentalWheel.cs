using UnityEngine;

namespace VehicularVanguard.Vehicle.Parts
{
    public class ExperimentalWheel : Part
    {
        SimpleWheel wheel;
        private void Awake()
        {
            wheel = GetComponent<SimpleWheel>();
            wheel.WheelSize = m_size;
        }

        public override void PartConnect(Part other)
        {
            Joint = gameObject.AddComponent<ConfigurableJoint>();
            Joint.connectedBody = other.Rigidbody;
            var j = (ConfigurableJoint)Joint;
            j.xMotion = j.yMotion = j.zMotion = j.angularZMotion = j.angularYMotion = ConfigurableJointMotion.Locked;
            // j.anchor = 
        }
    }
}
