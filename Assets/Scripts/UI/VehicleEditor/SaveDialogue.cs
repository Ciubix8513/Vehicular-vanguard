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
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VehicularVanguard.UI.Utils;
using VehicularVanguard.UI;
using System.Collections;
using VehicularVanguard.Player;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class SaveDialogue : MonoBehaviour
    {
        [SerializeField]
        private Button _ok;
        [SerializeField]
        private TMP_InputField _input;
        [SerializeField]
        private Camera _camera;
        public void TextChanged() => _ok.interactable = _input.text.Length > 0;
        public void Ok() => StartCoroutine(ok());
        private IEnumerator ok()
        {
            var name = _input.text;
            var path = Application.persistentDataPath + $"/Vehicles/{name}.vehicle";
            // var vehicle = FindObjectsOfType<Part>().Where(_ => _.isRoot).First();
            var vehicle = InputManager.PlayerVehicle.Root;
            if (!File.Exists(path))
            {
                _camera.gameObject.SetActive(true);
                //Let it render
                yield return null;
                _camera.gameObject.SetActive(false);
                var v = Saving.VehicleSaver.SerializeVehicle(vehicle, true, _camera.activeTexture);
                v.CreationDate = System.DateTime.Now.ToString();
                v.Name = name;
                Saving.VehicleSaver.SaveVehicle(v, _input.text);
                yield return null;
            }
            else
            {
                var modal = Instantiate(UIManager.ModalPrefab, UIManager.MainCanvas.transform).GetComponent<Modal>();
                modal.Init("File already exists\nReplace?");
                yield return new WaitUntil(() => modal.ButtonPressed);
                if (modal.Response)
                {
                    _camera.gameObject.SetActive(true);
                    //Let it render
                    yield return null;
                    _camera.gameObject.SetActive(false);
                    var v = Saving.VehicleSaver.SerializeVehicle(vehicle, true, _camera.activeTexture);
                    v.CreationDate = System.DateTime.Now.ToString();
                    v.Name = name;
                    Saving.VehicleSaver.SaveVehicle(v, _input.text);
                }
                Destroy(modal.gameObject);
            }
            _input.text = "";
            gameObject.SetActive(false);
        }
        void OnEnable() => InputManager.IgnoreInputs = true;
        void OnDisable() => InputManager.IgnoreInputs = false;
    }
}