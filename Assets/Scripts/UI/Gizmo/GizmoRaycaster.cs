using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoRaycaster : MonoBehaviour
{
    private static GameObject s_rotation;
    private static Camera s_camera;
    [SerializeField]
    private GameObject _rotationPrefab;
    private void Awake()
    {
        s_camera = Camera.main;
        s_rotation = Instantiate(_rotationPrefab);
        s_rotation.SetActive(false);
    }

    public void SwitchMode(int mode)
    {

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
            return;
        }
        if (!hit.collider.CompareTag("Gizmo")) return;
        Debug.Log($"Targeting {hit.collider.name}");
    }
}
