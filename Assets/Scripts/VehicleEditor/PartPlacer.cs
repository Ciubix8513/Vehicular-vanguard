using CarGame.Vehicle.Editor.UI;
using UnityEngine;
namespace CarGame.Vehicle.Editor
{
    public class PartPlacer : MonoBehaviour
    {
        static Camera s_camera;
        private void Start() => s_camera = Camera.main;
        public static void DoEditorRaycast()
        {
            if (!Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition), out var hit, 100.0f))
                return;
            if (!hit.collider.CompareTag("Part"))
                return;
            Debug.Log($"Should be dragging {hit.collider.gameObject.name}");
            var p = hit.collider.GetComponent<PartProxy>().part;
            if (p.isRoot) return;
            UICursor.IsDragging = true;
            UICursor.PartGO = p;
            if (UICursor.PartGO.transform.parent != null)
            {
                UICursor.PartGO.parentPart.attachedParts[-UICursor.PartGO.parentFace] = false;
                UICursor.PartGO.attachedParts[UICursor.PartGO.parentFace] = false;
                UICursor.PartGO.Joint.connectedBody = null;
            }
            UICursor.PartGO.transform.parent = null;
            UICursor.SetLayer(UICursor.PartGO.transform, 2);
        }
        //TODO pull out the stuff into separate functions 
        private void Update()
        {
            if (!UICursor.IsDragging)
                return;
            Ray ray = s_camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100.0f))
                return;
            if (!hit.collider.CompareTag("Part"))
            {
                UICursor.PartGO.transform.rotation = Quaternion.LookRotation(Vector3.forward, hit.normal);
                UICursor.PartGO.transform.position = hit.point + Vector3.up * UICursor.PartGO.m_size.y / 2;
                UICursor.SnappingObject = null;
                return;
            }
            Part part = hit.collider.GetComponent<PartProxy>().part;
            UICursor.SnappingObject = part;
            var p = hit.collider.transform.InverseTransformPoint(hit.point).normalized;
            bool ValidTargeting = false;
            Vector3 vec = Vector3.zero;
            Vector3Int dir = Vector3Int.zero;
            if (Mathf.Abs(p.x) > Mathf.Abs(p.y) && Mathf.Abs(p.x) > Mathf.Abs(p.z))
            {
                vec = new Vector3((part.m_size.x + UICursor.PartGO.m_size.x) / 2.0f, 0, 0) * Mathf.Sign(p.x);
                ValidTargeting = Mathf.Sign(p.x) < 0 ? !part.attachedParts.left : !part.attachedParts.right;
                dir = Vector3Int.right * ((int)Mathf.Sign(p.x));
            }
            if (Mathf.Abs(p.y) > Mathf.Abs(p.x) && Mathf.Abs(p.y) > Mathf.Abs(p.z))
            {
                vec = new Vector3(0, (part.m_size.y + UICursor.PartGO.m_size.y) / 2.0f, 0) * Mathf.Sign(p.y);
                ValidTargeting = Mathf.Sign(p.y) < 0 ? !part.attachedParts.down : !part.attachedParts.up;
                dir = Vector3Int.up * ((int)Mathf.Sign(p.y));
            }
            if (Mathf.Abs(p.z) > Mathf.Abs(p.y) && Mathf.Abs(p.z) > Mathf.Abs(p.x))
            {
                vec = new Vector3(0, 0, (part.m_size.z + UICursor.PartGO.m_size.z) / 2.0f) * Mathf.Sign(p.z);
                ValidTargeting = Mathf.Sign(p.z) < 0 ? !part.attachedParts.backward : !part.attachedParts.forward;
                dir = Vector3Int.forward * ((int)Mathf.Sign(p.z));
            }
            if (!ValidTargeting)
            {
                UICursor.SnappingObject = null;
                return;
            }
            UICursor.SnappingFace = dir;
            UICursor.PartGO.transform.rotation = part.transform.rotation;
            UICursor.PartGO.transform.position = part.transform.position + part.transform.rotation * vec;
        }
    }
}