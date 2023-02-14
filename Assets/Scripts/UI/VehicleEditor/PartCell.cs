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
using VehicularVanguard.Player;
using UnityEngine;
using UnityEngine.UI;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class PartCell : MonoBehaviour
    {
        private PartData _partData;
        public PartData PartData { get => _partData; set { _partData = value; GetComponent<Image>().sprite = _partData.sprite; } }
        public void MouseEnter() => UICursor.EnableNameCursor(PartData.name);
        public void MouesLeave()
        {
            UICursor.DisableNameCursor();
            //Assume dragging
            if (Input.GetMouseButton(0) && !UICursor.IsDragging && InputManager.editorMode == EditorMode.place)
                UICursor.StartDragging(_partData);
        }
    }
}