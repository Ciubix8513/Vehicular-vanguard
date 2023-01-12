using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarGame.UI.Utils;
using CarGame.UI;
using System.Collections;

namespace CarGame.Vehicle.Editor.UI
{
    public class SaveDialogue : MonoBehaviour
    {
        [SerializeField]
        private Button _ok;
        [SerializeField]
        private TMP_InputField _input;

        public void TextChanged() => _ok.interactable = _input.text.Length > 0;
        public void Ok()
        {
            StartCoroutine(ok());
        }
        private IEnumerator ok()
        {
            var path = Application.persistentDataPath + $"/Vehicles/{_input.text}.vehicle";
            var vehicle = FindObjectsOfType<Part>().Where(_ => _.isRoot).First();
            if (!File.Exists(path))
            {
                var v = Saving.VehicleSaver.SerializeVehicle(vehicle);
                Saving.VehicleSaver.SaveVehicle(v, _input.text);
                yield return null;
            }
            else
            {
                var modal = Instantiate(UIManager.ModalPrefab,UIManager.MainCanvas.transform).GetComponent<Modal>();
                modal.Init("File already exists\nReplace?");
                yield return new WaitUntil(() => modal.ButtonPressed);
                if(modal.Response)
                {
                    var v = Saving.VehicleSaver.SerializeVehicle(vehicle);
                    Saving.VehicleSaver.SaveVehicle(v,_input.text);
                }
                Destroy(modal.gameObject);
            }
            _input.text = "";
            gameObject.SetActive(false);
        }
    }
}