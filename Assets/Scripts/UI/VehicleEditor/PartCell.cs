using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PartCell : MonoBehaviour
{
    private PartData d;
    public PartData partData { get => d; set { d = value; GetComponent<Image>().sprite = d.sprite; } }
    public void MouseEnter()
    {
        //        Debug.Log($"Entered on {partData.name}");
        UICursor.EnableNameCursor(partData.name);
    }

    public void MouesLeave()
    {
        //       Debug.Log($"Exited on {partData.name}");
        UICursor.DisableNameCursor();
        //Assume dragging
        if (Input.GetMouseButton(0)&&!UICursor.IsDragging&&InputManager.editorMode == EditorMode.place)
            UICursor.StartDragging(d);


    }
}
