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
using UnityEngine;
using UnityEngine.UI;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class HistoryButtonInteractor : MonoBehaviour
    {
        [SerializeField]
        private Button _undoButton;
        [SerializeField]
        private Button _redoButton;

        public void Undo() => HistoryManager.ActionIndex--;
        public void Redo() => HistoryManager.ActionIndex++;
        void Awake() => HistoryManager.HistoryChangedEvent += ButtonUpdate;
        void OnEnable() => ButtonUpdate();
        //Disable buttons if history is unavailable
        private void ButtonUpdate()
        {
            if (_undoButton == null || _redoButton == null) return;
            _undoButton.interactable = _redoButton.interactable = true;
            if (Mathf.Abs(HistoryManager.HistoryLength) == 1)
                _undoButton.interactable = _redoButton.interactable = false;
            else if (HistoryManager.HistoryLength == HistoryManager.ActionIndex + 1)
                _redoButton.interactable = false;
            else if (HistoryManager.ActionIndex == 0)
                _undoButton.interactable = false;
        }
    }
}