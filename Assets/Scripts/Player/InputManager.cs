using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum Mode
    {
        editor,
        game,
        menu
    }
    public static Mode mode = Mode.game;

    public KeyCode EditorKey = KeyCode.B;


    [SerializeField]
    private GameObject EditorUI;
    void ProcessEK()
    {
        Debug.Log("Processing Editor key");
        if (mode == Mode.menu) return;
        if (mode == Mode.editor)
        {
            mode = Mode.game;
            EditorUI.SetActive(false);
            Time.timeScale = 1;
            return;
        }
        mode = Mode.editor;
        EditorUI.SetActive(true);
        Time.timeScale = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ProcessEK();
        if (Input.GetMouseButtonUp(0))
            UICursor.EndDragging();
    }

}
