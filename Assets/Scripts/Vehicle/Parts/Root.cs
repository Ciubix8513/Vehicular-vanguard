using UnityEngine;
public class Root : Part
{
    //Attach the root to the parent empty on start
    void Start()
    {   
        var parRB = transform.parent.GetComponent<Rigidbody>();
        var joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = parRB;
    }
}