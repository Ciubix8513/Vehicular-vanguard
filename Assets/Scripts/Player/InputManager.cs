using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;
using Cinemachine;
using CarGame.Vehicle.Editor;
using CarGame.Vehicle;
using CarGame.Vehicle.Editor.UI;

namespace CarGame.Player
{
    [Serializable]
    public enum Mode
    {
        editor,
        game,
        menu
    }
    [Serializable]
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

        [SerializeField]
        private GameObject _editorUI;

        [SerializeField]
        private UnityEvent OnEditorOpen = new();
        [SerializeField]
        private UnityEvent OnEditorClose = new();

        [SerializeField]
        private CinemachineVirtualCamera _editorCamera;

        [SerializeField]
        private CinemachineFreeLook _gameCamera;
        [SerializeField]
        private Transform _mainCamera;
        private static InputManager s_this;
        public static bool BlockLmb;
        void Awake() => s_this = this;
        void ProcessEK()
        {
            if (mode == Mode.menu) return;
            if (mode == Mode.editor)
            {
                GizmoRaycaster.HideGizmo();
                mode = Mode.game;
                _editorUI.SetActive(false);
                Time.timeScale = 1;
                OnEditorClose.Invoke();
                // editorMode = EditorMode.place;
                _editorCamera.Priority = 0;
                Cursor.lockState = CursorLockMode.Locked;
                return;
            }
            _editorCamera.transform.position = _mainCamera.transform.position;
            _editorCamera.transform.rotation = _mainCamera.transform.rotation;
            _editorCamera.Priority = 100;
            mode = Mode.editor;
            _editorUI.SetActive(true);
            Time.timeScale = 0;
            OnEditorOpen.Invoke();
            Cursor.lockState = CursorLockMode.None;
        }
        void Start()
        {
            if (mode != Mode.editor)
                ProcessEK();
        }
        void ProcessEditorModeKeys()
        {
            if (UICursor.IsDragging && Input.GetMouseButtonUp(0))
                UICursor.EndDragging();
            if (!BlockLmb)
            {
                if (!UICursor.IsDragging && Input.GetMouseButtonDown(0) && editorMode == EditorMode.place)
                    PartPlacer.DoEditorRaycast();
                if (Input.GetMouseButtonDown(0))
                    GizmoRaycaster.Raycast();
                if (editorMode == EditorMode.input && Input.GetMouseButtonDown(0))
                    InputMenu.DoRaycast();
            }
            if (GizmoRaycaster.Rotating && Input.GetMouseButtonUp(0))
                GizmoRaycaster.Rotating = false;
            if (!UICursor.IsDragging)
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    if (Input.GetKeyDown(KeyCode.Z))
                        HistoryManager.ActionIndex--;
                    else if (Input.GetKeyDown(KeyCode.Y))
                        HistoryManager.ActionIndex++;
                }
            }
        }
        void ProcessGameModeKeys()
        {
            PartGroups.DownGroup.group.Select(x => (x.Key, x.Value))
                                      .ToList()
                                      .ForEach(x => { if (Input.GetKeyDown(x.Key)) x.Value.ForEach(y => y.Item1()); });
            PartGroups.UpGroup.group.Select(x => (x.Key, x.Value))
                                    .ToList()
                                    .ForEach(x => { if (Input.GetKeyUp(x.Key)) x.Value.ForEach(y => y.Item1()); });
        }
        void Update()
        {
            if (Input.GetKeyDown(EditorKey))
                ProcessEK();
            switch (mode)
            {
                case Mode.editor:
                    ProcessEditorModeKeys();
                    break;
                case Mode.game:
                    ProcessGameModeKeys();
                    break;
            }
        }
        public static void SetGameCameraTarget(Transform target) => s_this._gameCamera.Follow = s_this._gameCamera.LookAt = target;
    }
}