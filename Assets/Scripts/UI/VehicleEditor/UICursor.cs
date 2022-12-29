using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

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
        UICursor.s_text = _localText;
        UICursor.s_image = _localImage;
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
            PartGO.gameObject.SetActive(false);
            s_image.gameObject.SetActive(true);
        }
        IsOverUI = true;
    }
    public static void ExitUI()
    {
        if (IsDragging)
        {
            PartGO.gameObject.SetActive(true);
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
        PartGO = Instantiate(data.prefab, Vector3.zero, Quaternion.identity).GetComponent<Part>();
        PartGO.partData = s_partData;
        s_originalLayers = new();
        SetLayer(PartGO.transform, 2);

        PartGO.gameObject.SetActive(false);
    }

    public static void EndDragging()
    {
        if (!PartGO.gameObject.activeSelf)
        {
            //Delete the part
            //     if (PartGO.binds.Count == 0) goto end;
            //     PartGO.binds.Select(x => x.Value).Where(x => x.Item2 == 0).ToList().ForEach(x => PartGroups.DownGroup.[x.Item1].RemoveAll(s => s.Item2 == PartGO.GetInstanceID()));
            //     PartGO.binds.Select(x => x.Value).Where(x => x.Item2 == 1).ToList().ForEach(x => PartGroups.UpGroup[x.Item1].RemoveAll(s => s.Item2 == PartGO.GetInstanceID()));
            // end:
            PartGroups.DownGroup.RemoveAllByInstanceId(PartGO.GetInstanceID());
            PartGroups.UpGroup.RemoveAllByInstanceId(PartGO.GetInstanceID());
            Destroy(PartGO.gameObject);
        }
        IsDragging = false;
        s_image.gameObject.SetActive(false);
        s_originalLayers.ForEach(x => { if (x.Item1 != null) x.Item1.layer = x.Item2;});
        if (SnappingObject == null)
        {
            Destroy(PartGO.GetComponent<FixedJoint>());
            return;
        }
        PartGO.gameObject.AddComponent<FixedJoint>().connectedBody = SnappingObject.GetComponent<Rigidbody>();
        PartGO.transform.parent = SnappingObject.transform.parent;
        PartGO.parentFace = SnappingFace * -1;
        PartGO.parentPart = SnappingObject;
        SnappingObject.attachedParts[SnappingFace] = true;
        PartGO.attachedParts[-SnappingFace] = true;
        PartGO = null;
    }
}
