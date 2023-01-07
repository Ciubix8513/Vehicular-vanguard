using System.Collections.Generic;
using UnityEngine;

namespace CarGame.UI
{
    public class UIManager : MonoBehaviour
    {
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
            EditorTabs = _editorTabs;
            EditorUI = _editorUI;
            EditorButtons = _editorButtons;
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