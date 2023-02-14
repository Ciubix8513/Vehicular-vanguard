//The GPLv3 License (GPLv3)
//
//Copyright (c) 2023 Ciubix8513
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>.
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