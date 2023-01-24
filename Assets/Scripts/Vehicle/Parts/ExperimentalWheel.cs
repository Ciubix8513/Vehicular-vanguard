using UnityEngine;

namespace CarGame.Vehicle.Parts
{
    public class ExperimentalWheel : Part
    {
        public override void PartConnect(Part other)
        {
            Joint = gameObject.AddComponent<ConfigurableJoint>();
            Joint.connectedBody = other.Rigidbody;
            var j = (ConfigurableJoint)Joint;
            j.xMotion = j.yMotion = j.zMotion = j.angularZMotion = j.angularYMotion = ConfigurableJointMotion.Locked;
        }
    }
}
