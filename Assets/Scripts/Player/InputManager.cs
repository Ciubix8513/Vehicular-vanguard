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
    public KeyCode Group0 = KeyCode.W;

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
        if (UICursor.isDragging && Input.GetMouseButtonUp(0))
            UICursor.EndDragging();
        if(!UICursor.isDragging && Input.GetMouseButtonDown(0) && mode == Mode.editor)
            PartPlacer.DoEditorRaycast();

        if(mode == Mode.game && Input.GetKeyDown(Group0))
        PartGroups.ProcessGroupDown(0);
        if(mode == Mode.game && Input.GetKeyUp(Group0))
        PartGroups.ProcessGroupUp(0);

    }

}
