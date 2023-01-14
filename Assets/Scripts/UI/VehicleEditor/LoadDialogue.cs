using System.IO;
using UnityEngine;
using System.Linq;
using CarGame.Vehicle.Saving;
using System.Collections.Generic;

namespace CarGame.Vehicle.Editor.UI
{

    public class LoadDialogue : MonoBehaviour
    {
        [SerializeField]
        private Transform _itemParent;
        [SerializeField]
        private GameObject _prefab;
        private List<LoadingMenuItem> _items;
        public void LoadSaves()
        {
            var path = Application.persistentDataPath + "/Vehicles/";
            var files = Directory.GetFiles(path).ToList().Where(_ => _.Split(".").Last().Trim() == "vehicle");
            //Get the list of compatible vehicles
            var vehicles = files.Select(_ => VehicleSaver.LoadVehicle(_, true))
            .Where(_ => _.Version.Compatible(new Version(Application.version))).ToList();
            //Create load items
            _items = new();
            vehicles.ForEach(_ => _items.Add(Instantiate(_prefab, _itemParent)
            .GetComponent<LoadingMenuItem>().Init(_.PreviewImage, _.Name, _.CreationDate)));

        }
    }
}