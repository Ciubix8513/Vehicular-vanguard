using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UICursor : MonoBehaviour
{

    static TextMeshProUGUI text;
    static Image image;
    public static bool isDragging = false;

    static PartData partData;
    public static Part partGO;
    public static Part SnappingObject;
    public static Vector3Int SnappingFace;
    [SerializeField] TextMeshProUGUI LocalText;
    [SerializeField] Image localImage;
    public static bool IsOverUI;
    void Awake()
    {
        UICursor.text = LocalText;
        UICursor.image = localImage;
    }
    public static void EnableNameCursor(string name)
    {
        text.text = name;
        text.gameObject.SetActive(true);
    }

    public static void DisableNameCursor()
    {
        text.gameObject.SetActive(false);
    }

    public static void EnterUI()
    {
        if (isDragging)
        {
            partGO.gameObject.SetActive(false);
            image.gameObject.SetActive(true);
        }
        IsOverUI = true;
    }
    public static void ExitUI()
    {
        if (isDragging)
        {
            partGO.gameObject.SetActive(true);
            image.gameObject.SetActive(false);
        }
        IsOverUI = false;
    }

    private void Update()
    {
        if (isDragging)
            image.transform.position = Input.mousePosition;
    }
    public static void StartDragging(PartData data)
    {
        partData = data;
        isDragging = true;
        image.sprite = data.sprite;
        image.gameObject.SetActive(true);
        partGO = Instantiate(data.prefab, Vector3.zero, Quaternion.identity).GetComponent<Part>();
        partGO.partData = partData;
        partGO.gameObject.layer = 2;
        partGO.gameObject.SetActive(false);
    }

    public static void EndDragging()
    {
        if (!partGO.gameObject.activeSelf)
            Destroy(partGO.gameObject);
        isDragging = false;
        image.gameObject.SetActive(false);
        partGO.gameObject.layer = 0;
        if (SnappingObject == null)
        {
            Destroy(partGO.GetComponent<FixedJoint>());
            return;
        }
        partGO.gameObject.AddComponent<FixedJoint>().connectedBody = SnappingObject.GetComponent<Rigidbody>();
        partGO.transform.parent = SnappingObject.transform.parent;
        partGO.parentFace = SnappingFace * -1;
        SnappingObject.attachedParts[SnappingFace] = true;
        partGO.attachedParts[-SnappingFace] = true;
        PartGroups.AddToGroup(0,partGO); 
        partGO = null;
    }
}
