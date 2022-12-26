using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
            cell.partData = part;
        }
    }
}
