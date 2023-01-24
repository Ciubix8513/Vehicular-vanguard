using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleWheel : MonoBehaviour
{
    private Rigidbody _rigidbody;
    void Awake() => _rigidbody = GetComponent<Rigidbody>();
    
}
