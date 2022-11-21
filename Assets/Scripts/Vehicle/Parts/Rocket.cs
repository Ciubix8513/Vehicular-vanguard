using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Part
{
    private ParticleSystem _particles;
    private Rigidbody _rb;
    public float force = 10.0f;
    public Vector3Int exhaustDir;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (isActive)
            _rb.AddForceAtPosition(transform.rotation * (Vector3)exhaustDir  * force,transform.position,ForceMode.Force);
    }
}
