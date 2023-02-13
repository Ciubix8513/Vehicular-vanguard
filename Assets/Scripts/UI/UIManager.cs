using System.Collections.Generic;
using VehicularVanguard.UI.Utils;
using UnityEngine;

namespace VehicularVanguard.UI
{
    public class UIManager : MonoBehaviour
    {
        //Prefabs
        [SerializeField]
        private GameObject _modal;
        public static GameObject ModalPrefab;
        //Local objects
        [SerializeField]
        private GameObject _canvas;
        public static GameObject MainCanvas;
        [SerializeField]
        private List<GameObject> _editorTabs;
        public static List<GameObject> EditorTabs;
        [SerializeField]
        private GameObject _editorUI;
        public static GameObject EditorUI;
        [SerializeField]
        private List<UnityEngine.UI.Button> _editorButtons;
        public static List<UnityEngine.UI.Button> EditorButtons;
        void Awake()
        {
            ModalPrefab =_modal;
            EditorTabs = _editorTabs;
            EditorUI = _editorUI;
            EditorButtons = _editorButtons;
            MainCanvas = _canvas;
        }
        public static void ActivateTab(uint tab)
        {
            if (tab > EditorTabs.Count)
            {
                Debug.LogException(new System.IndexOutOfRangeException());
                return;
            }
            EditorButtons.ForEach(_ => _.interactable = true);
            EditorTabs.ForEach(_ => _.SetActive(false));
            EditorTabs[(int)tab].SetActive(true);
            EditorButtons[(int)tab].interactable = false;
        }
    }
}