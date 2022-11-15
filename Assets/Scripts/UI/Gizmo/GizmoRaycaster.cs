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
    private static Part s_rotatingObj;
    private static Quaternion s_originalRotation;
    private static attachments s_originalAttachments;
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
        switch (mode)
        {
            //Rotation
            case 1:
                InputManager.editorMode = EditorMode.rotate;
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
            s_rotation.transform.position = hit.collider.transform.position;
            s_rotation.transform.rotation = hit.collider.transform.rotation;
            s_rotation.SetActive(true);
            s_rotatingObj = hit.collider.GetComponent<Part>();
            s_originalRotation = s_rotatingObj.transform.localRotation;
            s_originalAttachments = s_rotatingObj.attachedParts;
            return;
        }
        if (!hit.collider.CompareTag("Gizmo")) return;
        Debug.Log($"Targeting {hit.collider.name}");
        Rotating = true;
        s_delta = 0;
        s_originalRotation = s_rotatingObj.transform.localRotation;
        if (hit.collider.name.ToLower() == "x")
            s_axis = 0;
        else if (hit.collider.name.ToLower() == "y")
            s_axis = 1;
        else
            s_axis = 2;
    }
    private void Update()
    {
        if (!Rotating) return;
        s_delta += Input.GetAxis(s_axis == 1 ? "Mouse X" : "Mouse Y");
        if (s_axis == 0)
        {
            //X axis
            var num = ((int)s_delta / 15) % 4 * (int)Mathf.Sign(s_delta);
            attachments attachments = s_originalAttachments;
            for (int i = 0; i < 4 + num; i++)
                attachments = attachments.RotateX();
            // s_rotatingObj.attachedParts = attachments;
            s_rotatingObj.transform.localRotation = Quaternion.Euler(s_originalRotation.eulerAngles + new Vector3(90.0f * (((int)s_delta) / 15), 0, 0));
        }
        else if (s_axis == 1)
        {
            //Y axis
            var num = ((int)s_delta / 15) % 4 * (int)Mathf.Sign(s_delta);
            attachments attachments = s_originalAttachments;
            for (int i = 0; i < 4 + num; i++)
                attachments = attachments.RotateY();
            // s_rotatingObj.attachedParts = attachments;
            s_rotatingObj.transform.localRotation = Quaternion.Euler(s_originalRotation.eulerAngles + new Vector3(0, 90.0f * (((int)s_delta) / 15), 0));
        }
        else
        {
            //Z axis
            var num = ((int)s_delta / 15) % 4 * (int)Mathf.Sign(s_delta);
            attachments attachments = s_originalAttachments;
            for (int i = 0; i < 4 + num; i++)
                attachments = attachments.RotateZ();
            // s_rotatingObj.attachedParts = attachments;
            s_rotatingObj.transform.localRotation = Quaternion.Euler(s_originalRotation.eulerAngles + new Vector3(0, 0, 90.0f * num));

        }
    }
}