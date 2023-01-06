using UnityEngine;

public class GizmoRaycaster : MonoBehaviour
{
    public static bool Rotating = false;
    private static float s_delta;
    private static GameObject s_rotation;
    private static Camera s_camera;
    [SerializeField]
    private GameObject _rotationPrefab;
    private static int s_axis;
    private static PartProxy s_rotatingObj;
    private static Quaternion s_originalRotation;
    private static Attachments s_originalAttachments;
    private static int s_oldLayer;
    private void Awake()
    {
        s_camera = Camera.main;
        s_rotation = Instantiate(_rotationPrefab);
        s_rotation.SetActive(false);
    }
    public static void HideGizmo() => s_rotation.SetActive(false);
    public void SwitchMode(int mode)
    {
        Rotating = false;
        if (s_rotatingObj!= null)
            s_rotatingObj.gameObject.layer = s_oldLayer;
        switch (mode)
        {
            case 1:
                InputManager.editorMode = EditorMode.rotate;
                break;
            case 2:
                InputManager.editorMode = EditorMode.input;
                s_rotation.SetActive(false);
                break;
            default:
                InputManager.editorMode = EditorMode.place;
                s_rotation.SetActive(false);
                break;
        }
    }
    public static void Raycast()
    {
        if (InputManager.editorMode != EditorMode.rotate) return;
        if (!Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition), out var hit, 500.0f)) return;

        if (hit.collider.CompareTag("Part"))
        {
            if (s_rotatingObj != null)
                s_rotatingObj.gameObject.layer = s_oldLayer;
            s_oldLayer = hit.collider.gameObject.layer;
            hit.collider.gameObject.layer = 2;
            s_rotation.transform.position = hit.collider.transform.position;
            s_rotation.transform.rotation = hit.collider.transform.rotation;
            s_rotation.SetActive(true);
            s_rotatingObj = hit.collider.GetComponent<PartProxy>();
            s_originalRotation = s_rotatingObj.part.transform.localRotation;
            s_originalAttachments = s_rotatingObj.part.attachedParts;
            return;
        }
        if (!hit.collider.CompareTag("Gizmo")) return;
        Rotating = true;
        s_delta = 0;
        s_originalRotation = s_rotatingObj.part.transform.localRotation;
        if (hit.collider.name.ToLower() == "x")
            s_axis = 0;
        else if (hit.collider.name.ToLower() == "y")
            s_axis = 1;
        else
            s_axis = 2;
        Rotate(s_axis);
    }
    static void Rotate(int axis)
    {
        var parts = s_rotatingObj.part.attachedParts;
        parts[s_rotatingObj.part.parentFace] = false;
        if (axis == 0)
            s_rotatingObj.part.transform.Rotate(new Vector3(Input.GetKey(KeyCode.LeftShift) ? -90 : 90, 0, 0));
        else if (axis == 1)
            s_rotatingObj.part.transform.Rotate(new Vector3(0, Input.GetKey(KeyCode.LeftShift) ? -90 : 90, 0));
        else
            s_rotatingObj.part.transform.Rotate(new Vector3(0, 0, Input.GetKey(KeyCode.LeftShift) ? -90 : 90));
        //This is a pretty dumb fix but it should work
        var body = s_rotatingObj.part.Joint.connectedBody;
        Destroy(s_rotatingObj.part.Joint);
        (s_rotatingObj.part.Joint = s_rotatingObj.gameObject.AddComponent<FixedJoint>()).connectedBody = body;
        CarGame.Vehicle.Editor.HistoryManager.ProcessChange("Rotating a part");
    }
}