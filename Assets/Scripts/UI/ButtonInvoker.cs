using UnityEngine;

public class ButtonInvoker : MonoBehaviour
{
    private UnityEngine.UI.Button _btn;
    private void Awake() => _btn = GetComponent<UnityEngine.UI.Button>();
    public void InvokeButton() => _btn.onClick.Invoke();
}
