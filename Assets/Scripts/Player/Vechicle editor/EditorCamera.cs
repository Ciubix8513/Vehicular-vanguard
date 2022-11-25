using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    public float MouseSensitivity = 1.0f;
    public float speed = 0.01f;
    void Update()
    {
        if(InputManager.mode != Mode.editor)
            return;
        if (Input.GetButtonDown("Fire2"))
            Cursor.lockState = CursorLockMode.Locked;
        if (Cursor.lockState != CursorLockMode.Locked)
            return;
        
        if (Input.GetButtonUp("Fire2"))
            Cursor.lockState = CursorLockMode.None;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles +
         new Vector3(-1.0f * Input.GetAxis("Mouse Y"),
         Input.GetAxis("Mouse X"), 0.0f) * MouseSensitivity * Time.unscaledDeltaTime);
        Vector3 inp = new Vector3(
            (Input.GetKey(KeyCode.D) == true ? 1 : 0) - (Input.GetKey(KeyCode.A) == true ? 1 : 0),
            (Input.GetKey(KeyCode.E) == true ? 1 : 0) - (Input.GetKey(KeyCode.Q) == true ? 1 : 0),
            (Input.GetKey(KeyCode.W) == true ? 1 : 0) - (Input.GetKey(KeyCode.S) == true ? 1 : 0));
        transform.localPosition += transform.rotation * inp * speed * Time.unscaledDeltaTime * (Input.GetKey(KeyCode.LeftShift) == true ? 2 : 1);
    }
}
