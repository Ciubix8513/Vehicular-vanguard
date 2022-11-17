using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameText : MonoBehaviour
{
    [SerializeField]
    Vector3 offset = Vector3.zero;

    void Update()
    {
        transform.position = Input.mousePosition + offset;
    }
}
