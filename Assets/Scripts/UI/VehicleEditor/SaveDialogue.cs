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