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
using System.Collections;
using TMPro;
using UnityEngine;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class ActionCell : MonoBehaviour
    {
        public TextMeshProUGUI Name;
        public Part.ActionDel del;
        [SerializeField]
        private TextMeshProUGUI _keyText;
        [SerializeField]
        private TMPro.TMP_Dropdown _dropdown;
        private KeyCode _key = KeyCode.W;
        private Part _part;
        [SerializeField]
        int _downOption;
        private InputMenu _parent;
        public void Init(ActionData data, Part p, InputMenu par)
        {
            _parent = par;
            Name.text = data.Name;
            del = data.Delegate;
            _key = data.Key;
            _dropdown.SetValueWithoutNotify(data.ActionType);
            _part = p;
            _keyText.text = _key.ToString();
            if (p.binds.Count == 0) return;
            _key = p.binds[data.Name].Item1;
            _dropdown.value = p.binds[data.Name].Item2;
            _keyText.text = _key.ToString();
        }

        public void GetKey() => StartCoroutine(getKey());
        private IEnumerator getKey()
        {
            while (!Input.anyKeyDown)
                yield return null;
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                if (Input.GetKey(key))
                {
                    _key = key;
                    goto end;
                }
            end:
            _keyText.text = _key.ToString();
            SaveAll();
        }
        public void SaveAll()
        {
            if (_parent == null)
                return;
            _parent.SaveActions();
            HistoryManager.ProcessChange("Action cell change");
        }

        public void Save()
        {
            PartGroups.Instance[_dropdown.value].Add(_key, (del, _part.GetInstanceID()));
            if (!_part.binds.ContainsKey(Name.text))
                _part.binds.Add(Name.text, new(_key, _dropdown.value));
        }
    }
}