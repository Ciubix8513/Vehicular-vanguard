using System.IO;
using UnityEngine;
using System.Linq;
using VehicularVanguard.Vehicle.Saving;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace VehicularVanguard.Vehicle.Editor.UI
{

    public class LoadDialogue : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _itemParent;
        [SerializeField]
        private GameObject _prefab;
        [SerializeField]
        private Button _delete;
        [SerializeField]
        private Button _load;

        private List<LoadingMenuItem> _items;
        private LoadingMenuItem _selectedItem;

        public LoadingMenuItem SelectedItem
        {
            get => _selectedItem; set
            {
                _selectedItem = value;
                _items.ForEach(_ => _.Outline.SetActive(false));
                if (_selectedItem != null)
                    _items.Where(_ => _.GetInstanceID() == _selectedItem.GetInstanceID())
                    .First().Outline.SetActive(true);
                _delete.interactable = _load.interactable = true;
            }
        }
        public void DeleteSelected() => _selectedItem.Delete();
        public void LoadSelected() => _selectedItem.Load();

        void OnEnable() => LoadSaves();
        public void LoadSaves()
        {
            _delete.interactable = _load.interactable = false;
            //Remove all existing items
            foreach (Transform c in _itemParent.transform)
            {
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
            .GetComponent<LoadingMenuItem>().Init(_, this)));
            var gr = _itemParent.GetComponent<GridLayoutGroup>();

            _itemParent.sizeDelta = new Vector2(_itemParent.sizeDelta.x, (gr.cellSize.y + gr.spacing.y) * vehicles.Count + gr.padding.top + gr.padding.bottom);
        }
    }
}