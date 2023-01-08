using UnityEngine;
using TMPro;

namespace CarGame.UI.Utils
{
    public class Modal : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _textObj;
        [SerializeField]
        private TextMeshProUGUI _leftButton;
        [SerializeField]
        private TextMeshProUGUI _rightButton;
        public bool ButtonPressed = false;
        public bool Response = false;
        void OnEnable() => ButtonPressed = false;
        public void Init(string text, string leftButtonText = "yes", string rightButtonText = "no")
        {
            _textObj.text = text;
            _leftButton.text = leftButtonText;
            _rightButton.text = rightButtonText;
            this.gameObject.SetActive(true);
        }
        public void Press(bool val)
        {
            ButtonPressed = true;
            Response = val;
        }
    }
}