using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public enum Mode
{
    editor,
    game,
    menu
}
[System.Serializable]
public enum EditorMode
{
    place,
    rotate,
    input
}

public class InputManager : MonoBehaviour
{
    public static Mode mode = Mode.game;
    public static EditorMode editorMode = EditorMode.place;
    public KeyCode EditorKey = KeyCode.B;
    public KeyCode Group0 = KeyCode.W;

    [SerializeField]
    private GameObject EditorUI;

    [SerializeField]
    private UnityEvent OnEditorOpen = new();
    [SerializeField]
    private UnityEvent OnEditorClose = new();
    void ProcessEK()
    {
        Debug.Log("Processing Editor key");
        if (mode == Mode.menu) return;
        if (mode == Mode.editor)
        {
            GizmoRaycaster.HideGizmo();
            mode = Mode.game;
            EditorUI.SetActive(false);
            Time.timeScale = 1;
            OnEditorClose.Invoke();
            return;
        }
        mode = Mode.editor;
        EditorUI.SetActive(true);
        Time.timeScale = 0;
        OnEditorOpen.Invoke();
    }
    void Start()
    {
        ProcessEK();
    }
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.B))
            ProcessEK();
        if (UICursor.IsDragging && Input.GetMouseButtonUp(0))
            UICursor.EndDragging();
        if (!UICursor.IsDragging && Input.GetMouseButtonDown(0) && mode == Mode.editor && editorMode == EditorMode.place)
            PartPlacer.DoEditorRaycast();
        if (mode == Mode.editor && Input.GetMouseButtonDown(0))
            GizmoRaycaster.Raycast();
        if (mode == Mode.editor && editorMode == EditorMode.input && Input.GetMouseButtonDown(0))
            InputMenu.DoRaycast();
        if (mode == Mode.editor && GizmoRaycaster.Rotating && Input.GetMouseButtonUp(0))
            GizmoRaycaster.Rotating = false;
        if (mode == Mode.game)
        {
            PartGroups.DownGroup.Select(x => new Tuple<KeyCode, List<Tuple<Part.ActionDel,int>>>(x.Key, x.Value)).ToList().ForEach(x => { if (Input.GetKeyDown(x.Item1)) x.Item2.ForEach(y => y.Item1()); });
            PartGroups.UpGroup.Select(x => new Tuple<KeyCode, List<Tuple<Part.ActionDel,int>>>(x.Key, x.Value)).ToList().ForEach(x => { if (Input.GetKeyUp(x.Item1)) x.Item2.ForEach(y => y.Item1()); });
        }
    }

}
