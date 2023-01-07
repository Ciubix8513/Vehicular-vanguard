using UnityEngine;
using UnityEngine.UI;
using CarGame.Vehicle.Editor;

public class HistoryButtonInteractor : MonoBehaviour
{
    [SerializeField]
    private Button _undoButton;
    [SerializeField]
    private Button _redoButton;

    public void Undo() => HistoryManager.ActionIndex--;
    public void Redo() => HistoryManager.ActionIndex++;
    void Awake() => HistoryManager.HistoryChangedEvent += ButtonUpdate;
    void OnEnable() => ButtonUpdate();
    //Disable buttons if history is unavailable
    private void ButtonUpdate()
    {
        if(_undoButton == null || _redoButton == null)return;
        _undoButton.interactable = _redoButton.interactable = true;
        if( Mathf.Abs(HistoryManager.HistoryLength) == 1)
            _undoButton.interactable = _redoButton.interactable = false;
        else if(HistoryManager.HistoryLength == HistoryManager.ActionIndex + 1)
            _redoButton.interactable = false;
        else if(HistoryManager.ActionIndex == 0)
            _undoButton.interactable = false;
    }
}
