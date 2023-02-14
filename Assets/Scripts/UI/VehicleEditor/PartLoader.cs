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
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace VehicularVanguard.Vehicle.Editor.UI
{
    public class PartLoader : MonoBehaviour
    {
        public GameObject CellPrefab;
        public RectTransform cellParent;
        public static bool IncludeExcludedParts = false;

        void Start() => LoadParts();
        void LoadParts()
        {
            var parts = PartDictionary.Parts.Select(_ => _.Value).Where(_ => !_.ExcludeFromBuildMenu || IncludeExcludedParts).ToArray();
            var grid = cellParent.GetComponent<GridLayoutGroup>();
            cellParent.sizeDelta = parts.Length / Mathf.Floor(cellParent.parent.parent.GetComponent<RectTransform>().sizeDelta.x / (grid.cellSize.x + grid.spacing.x)) * new Vector2(0, grid.cellSize.y + grid.spacing.y) + new Vector2(0, grid.padding.top * 4);
            foreach (var part in parts)
            {
                var cell = Instantiate(CellPrefab, Vector3.zero, Quaternion.identity, cellParent).GetComponent<PartCell>();
                cell.PartData = part;
            }
        }
    }
}