using System.IO;
using UnityEngine;
using System.Linq;
using CarGame.Vehicle.Saving;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace CarGame.Vehicle.Editor.UI
{

    public class LoadDialogue : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _itemParent;
        [SerializeField]
        private GameObject _prefab;
        private List<LoadingMenuItem> _items;
        void Start() => LoadSaves();
        public void LoadSaves()
        {
            //Remove all existing items
            foreach(Transform c in _itemParent.transform){
                Destroy(c.gameObject);
            }
            var path = Application.persistentDataPath + "/Vehicles/";
            var files = Directory.GetFiles(path).ToList().Where(_ => _.Split(".").Last().Trim() == "vehicle");
            //Get the list of compatible vehicles
            var vehicles = files.Select(_ => VehicleSaver.LoadVehicle(_, true))
            .Where(_ => _.Version.Compatible(new Saving.Version(Application.version))).ToList();
            //Create load items
            _items = new();
            vehicles.ForEach(_ => _items.Add(Instantiate(_prefab, _itemParent)
            .GetComponent<LoadingMenuItem>().Init(_,this)));
            var gr = _itemParent.GetComponent<GridLayoutGroup>();

            _itemParent.sizeDelta = new Vector2(_itemParent.sizeDelta.x, (gr.cellSize.y + gr.spacing.y) * vehicles.Count + gr.padding.top + gr.padding.bottom);
        }
    }
}