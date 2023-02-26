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
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class UICursor : MonoBehaviour
    {
        private static TextMeshProUGUI s_text;
        private static Image s_image;
        public static bool IsDragging = false;
        private static PartData s_partData;
        public static Part PartGO;
        public static Part SnappingObject;
        public static Vector3Int SnappingFace;
        [SerializeField]
        private TextMeshProUGUI _localText;
        [SerializeField]
        private Image _localImage;

        private static List<System.Tuple<GameObject, int>> s_originalLayers;
        public static bool IsOverUI;
        void Awake()
        {
            s_text = _localText;
            s_image = _localImage;
        }
        public static void EnableNameCursor(string name)
        {
            if (IsDragging)
                return;
            s_text.text = name;
            s_text.gameObject.SetActive(true);
        }
        public static void DisableNameCursor() => s_text.gameObject.SetActive(false);
        public static void EnterUI()
        {
            if (IsDragging)
            {
                PartGO.DraggingObject.gameObject.SetActive(false);
                s_image.gameObject.SetActive(true);
            }
            IsOverUI = true;
        }
        public static void ExitUI()
        {
            if (IsDragging)
            {
                PartGO.DraggingObject.gameObject.SetActive(true);
                s_image.gameObject.SetActive(false);
            }
            IsOverUI = false;
        }

        private void Update()
        {
            if (IsDragging)
                s_image.transform.position = Input.mousePosition;
        }
        public static void SetLayer(Transform t, int layer)
        {
            s_originalLayers.Add(new(t.gameObject, t.gameObject.layer));
            t.gameObject.layer = layer;
            foreach (Transform c in t)
                SetLayer(c, layer);
        }
        public static void StartDragging(PartData data)
        {
            s_partData = data;
            IsDragging = true;
            s_image.sprite = data.sprite;
            s_image.gameObject.SetActive(true);
            PartGO = Instantiate(data.prefab, Vector3.zero, Quaternion.identity).GetComponentInChildren<Part>();
            PartGO.partData = s_partData;
            s_originalLayers = new();

            PartGO.SetProxiesLayer(2);
            // SetLayer(PartGO.transform, 2);

            PartGO.DraggingObject.gameObject.SetActive(false);
        }

        public static void EndDragging()
        {
            if (!PartGO.DraggingObject.gameObject.activeSelf)
            {
                PartGroups.Instance[0].RemoveAllByInstanceId(PartGO.GetInstanceID());
                PartGroups.Instance[1].RemoveAllByInstanceId(PartGO.GetInstanceID());
                Destroy(PartGO.gameObject);
            }
            IsDragging = false;
            s_image.gameObject.SetActive(false);
            s_originalLayers.ForEach(x => { if (x.Item1 != null) x.Item1.layer = x.Item2; });
            if (SnappingObject == null)
            {
                Destroy(PartGO.Joint);
                HistoryManager.ProcessChange("Destroying a part");
                return;
            }
            PartGO.PartConnect(SnappingObject);
            PartGO.transform.parent = SnappingObject.transform.parent;
            PartGO.parentFace = SnappingFace * -1;
            PartGO.parentPart = SnappingObject;
            SnappingObject.attachedParts[SnappingFace] = true;
            PartGO.attachedParts[-SnappingFace] = true;
            if (PartGO.WasPlaced)
                goto end;
            PartGO.WasPlaced = true;
            if (!PartGO.Activatable)
                goto end;
            print("Initializing input for a new object");
            PartGO.GetActions().ForEach(_ =>
            {
                PartGroups.Instance[_.ActionType].Add(_.Key, (_.Delegate, PartGO.GetInstanceID()));
                PartGO.binds.Add(_.Name, new(_.Key, _.ActionType));
            });

        //Using a goto because it's the simplest way
        end:
            PartGO = null;
            HistoryManager.ProcessChange("Moving/Creating a part");
        }
    }
}