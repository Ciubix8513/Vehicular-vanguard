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
using UnityEngine.UI;
using System.IO;
using VehicularVanguard.Vehicle.Saving;
using System.Linq;
using VehicularVanguard.Player;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class LoadingMenuItem : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _name;
        [SerializeField]
        TextMeshProUGUI _date;
        [SerializeField]
        Image _image;
        public GameObject Outline;
        private Saving.Vehicle vehicle;
        LoadDialogue parent;
        public void Delete()
        {
            File.Delete(vehicle.FileName);
            parent.LoadSaves();
        }
        public void Load()
        {
            var Root = VehicleSaver.GenerateVehicle(
            vehicle,
            // FindObjectsOfType<Part>().ToList().Where(_ => _.isRoot).First().transform.parent,
            InputManager.PlayerVehicle.transform,
            null,
            out _);
            InputManager.SetGameCameraTarget(Root.transform);
            InputManager.PlayerVehicle.Root = Root;
            HistoryManager.ResetHistory();
            parent.gameObject.SetActive(false);
        }
        public void Select() => parent.SelectedItem = this;
        public LoadingMenuItem Init(Saving.Vehicle v, LoadDialogue par)
        {
            Texture2D tex = new(1, 1);
            tex.LoadImage(v.PreviewImage);
            _name.text = v.Name;
            _date.text = "Created: " + v.CreationDate;
            _image.sprite = Sprite.Create(tex,
                                          new Rect(0, 0, tex.width, tex.height),
                                          new Vector2(0.5f, 0.5f));
            vehicle = v;
            //Remove what I don't need anymore
            v.PreviewImage = new byte[0];
            parent = par;
            return this;
        }
    }
}