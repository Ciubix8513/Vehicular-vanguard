using CarGame.Player;
using UnityEngine;
using UnityEngine.UI;

namespace CarGame.Vehicle.Editor.UI
{
    public class PartCell : MonoBehaviour
    {
        private PartData _partData;
        public PartData PartData { get => _partData; set { _partData = value; GetComponent<Image>().sprite = _partData.sprite; } }
        public void MouseEnter() => UICursor.EnableNameCursor(PartData.name);
        public void MouesLeave()
        {
            UICursor.DisableNameCursor();
            //Assume dragging
            if (Input.GetMouseButton(0) && !UICursor.IsDragging && InputManager.editorMode == EditorMode.place)
                UICursor.StartDragging(_partData);
        }
    }
}