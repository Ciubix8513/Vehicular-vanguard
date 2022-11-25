using UnityEngine;
using UnityEngine.UI;
public class PartCell : MonoBehaviour
{
    private PartData _partData;
    public PartData partData { get => _partData; set { _partData = value; GetComponent<Image>().sprite = _partData.sprite; } }
    public void MouseEnter()=>UICursor.EnableNameCursor(partData.name);
    public void MouesLeave()
    {
        UICursor.DisableNameCursor();
        //Assume dragging
        if (Input.GetMouseButton(0)&&!UICursor.IsDragging&&InputManager.editorMode == EditorMode.place)
            UICursor.StartDragging(_partData);
    }
}
