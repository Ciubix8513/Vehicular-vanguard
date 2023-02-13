using UnityEngine;

namespace VehicularVanguard.Vehicle.Parts
{
    public class Root : Part
    {
        //Attach the root to the parent empty on start
        void Start()
        {
            var parRB = transform.parent.GetComponent<Rigidbody>();
            Joint = gameObject.AddComponent<FixedJoint>();
            Joint.connectedBody = parRB;
        }
    }
}