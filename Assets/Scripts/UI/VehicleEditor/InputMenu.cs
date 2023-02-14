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
using System.Linq;
using UnityEngine;
using TMPro;
using VehicularVanguard.Player;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class InputMenu : MonoBehaviour
    {
        //----UI fields----
        [SerializeField]
        private GameObject _actionPrefab;
        [SerializeField]
        private GameObject _actionParent;
        [SerializeField]
        TextMeshProUGUI _nameText;
        [SerializeField]
        TextMeshProUGUI _descText;
        //----Data----
        private List<Part> _parts = new();
        private static Camera s_camera;
        public static InputMenu Instance;
        private Part _part = null;
        private bool _selected;
        private static int s_selectedId;
        public static int SelectedId { get => s_selectedId; }
        private Dictionary<int, int> _originalLayers;
        void Awake()
        {
            s_camera = Camera.main;
            Instance = this;
            _parts = new();
        }
        public void OnMenuOpen()
        {
            _originalLayers = new();
            _parts = FindObjectsOfType<Part>().Where(_ => _.Activatable).ToList();
            _parts.ForEach(_=>_.SetProxiesLayer(6,true));
            foreach (Transform c in _actionParent.transform)
                Destroy(c.gameObject);
            _nameText.text = _descText.text = "";
            _selected = false;
            s_selectedId = 0;
        }
        public void OnMenuClose(bool deselect)
        {
            _parts.Where(_=>_!=null).ToList().ForEach(_ => _.RestoreProxiesLayers());
            if(!deselect)return;
            _selected = false;
            s_selectedId = 0;
        }
        public void RestoreClosed()
        {
            if(InputManager.editorMode != EditorMode.input)return;
            var selBack = _selected;
            OnMenuOpen();
            if(selBack)
            LoadData(_part,false);
        }
        public static void DoRaycast()
        {
            if (!Physics.Raycast(s_camera.ScreenPointToRay(Input.mousePosition),
                                 out var hit,
                                 100.0f,
                                 1 << 6)) return;
            if (!hit.collider.CompareTag("Part")) return;
            Instance.LoadData(hit.collider.GetComponent<PartProxy>().part, false);
        }
        public void LoadData(Part part, bool ignoreHistory)
        {
            if (_selected)
                SaveActions();
            _selected = true;
            s_selectedId = part.GetHashCode();
            if(_part != null)
            _part.SetProxiesLayer(6);
            _part = part;
            part.SetProxiesLayer(9);
            _nameText.text = _part.partData.name;
            _descText.text = _part.partData.description;

            foreach (Transform c in _actionParent.transform)
                Destroy(c.gameObject);
            _part.GetActions().ForEach(a => Instantiate(_actionPrefab,
                Vector3.zero,
                Quaternion.identity,
                _actionParent.transform).GetComponent<ActionCell>().Init(a, _part, this));
            if (_part.binds.Count == 0) goto end;
            PartGroups.Instance[0].RemoveAllByInstanceId(_part.GetInstanceID());
            PartGroups.Instance[1].RemoveAllByInstanceId(_part.GetInstanceID());
            _part.binds.Clear();
        end:
            SaveActions();
            if (!ignoreHistory)
                HistoryManager.ProcessChange("Input menu part loading");
        }
        public void SaveActions()
        {
            if (InputManager.editorMode != EditorMode.input || _part == null) return;
            _part.binds.Clear();
            foreach (Transform c in _actionParent.transform)
                c.GetComponent<ActionCell>().Save();

        }
    }
}