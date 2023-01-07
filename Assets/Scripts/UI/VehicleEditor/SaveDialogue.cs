using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
            var path = Application.persistentDataPath + $"Vehicles/{_input.text}.vehicle";
            var vehicle = FindObjectsOfType<Part>().Where(_ => _.isRoot).First();
            if(!File.Exists(path))
            {
                var v = Saving.VehicleSaver.SerializeVehicle(vehicle);
                Saving.VehicleSaver.SaveVehicle(v,_input.text);
            }
        }
    }
}