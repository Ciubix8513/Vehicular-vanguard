using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPlacer : MonoBehaviour
{
    Camera camera;
    private void Start()
    {
        camera = Camera.main;
    }
    private void Update()
    {
        if (!UICursor.isDragging)
            return;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit, 100.0f))
            return;
        if (!hit.collider.CompareTag("Part"))
        {
            UICursor.partGO.transform.position = hit.point;
            UICursor.SnappingObject = null;
            return;
        }
        var part = hit.collider.GetComponent<Part>();
        UICursor.SnappingObject = part;
        var p = hit.collider.transform.InverseTransformPoint(hit.point).normalized;
        Vector3 vec = Vector3.zero;
        if (Mathf.Abs(p.x) > Mathf.Abs(p.y) && Mathf.Abs(p.x) > Mathf.Abs(p.z))
        {
            vec = new Vector3((part.m_size.x + UICursor.partGO.m_size.x) / 2.0f, 0, 0) * Mathf.Sign(p.x);
            UICursor.partGO.transform.rotation = part.transform.rotation;
            UICursor.partGO.transform.position = part.transform.position + part.transform.rotation * vec;
        }

        if (Mathf.Abs(p.y) > Mathf.Abs(p.x) && Mathf.Abs(p.y) > Mathf.Abs(p.z))
        {
            vec =  new Vector3(0,(part.m_size.y + UICursor.partGO.m_size.y) / 2.0f,  0) * Mathf.Sign(p.y);
            UICursor.partGO.transform.rotation = part.transform.rotation;
            UICursor.partGO.transform.position = part.transform.position + part.transform.rotation * vec;
            Debug.Log(vec);
        }
        if (Mathf.Abs(p.z) > Mathf.Abs(p.y) && Mathf.Abs(p.z) > Mathf.Abs(p.x))
        {
            vec = new Vector3(0,0,(part.m_size.z + UICursor.partGO.m_size.z) / 2.0f) * Mathf.Sign(p.z);
            UICursor.partGO.transform.rotation = part.transform.rotation;
            UICursor.partGO.transform.position = part.transform.position + part.transform.rotation * vec;
        }
    }
}
