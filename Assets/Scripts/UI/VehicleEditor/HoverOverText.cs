using System.Collections;
using CarGame.Vehicle.Editor.UI;
using UnityEngine;

namespace CarGame.UI.Utils
{
    public class HoverOverText : MonoBehaviour
    {
        [SerializeField]
        private GameObject _text;
        [SerializeField]
        private int _textDelay = 1;
        public void MouseEnter()
        {
            if (UICursor.IsDragging) return;
            StartCoroutine(nameof(MouseEnterCoroutine));
        }
        private IEnumerator MouseEnterCoroutine()
        {
            yield return new WaitForSecondsRealtime(_textDelay);
            _text.SetActive(true);
        }
        public void MouseLeave()
        {
            StopAllCoroutines();
            _text.SetActive(false);
        }
    }
}