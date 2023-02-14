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
using TMPro;

namespace VehicularVanguard.UI.Utils
{
    public class Modal : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _textObj;
        [SerializeField]
        private TextMeshProUGUI _leftButton;
        [SerializeField]
        private TextMeshProUGUI _rightButton;
        public bool ButtonPressed = false;
        public bool Response = false;
        void OnEnable() => ButtonPressed = false;
        public void Init(string text, string leftButtonText = "yes", string rightButtonText = "no")
        {
            _textObj.text = text;
            _leftButton.text = leftButtonText;
            _rightButton.text = rightButtonText;
            this.gameObject.SetActive(true);
        }
        public void Press(bool val)
        {
            ButtonPressed = true;
            Response = val;
        }
    }
}