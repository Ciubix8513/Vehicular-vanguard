using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CarGame.Vehicle.Editor.UI
{
    public class LoadingMenuItem : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _name;
        [SerializeField]
        TextMeshProUGUI _date;
        [SerializeField]
        Image _image;
        public LoadingMenuItem Init(byte[] img, string name, string date)
        {
            Texture2D tex = new(1, 1);
            tex.LoadImage(img);
            _name.text = name;
            _date.text = date;
            _image.sprite = Sprite.Create(tex,
                                          new Rect(0, 0, tex.width, tex.height),
                                          new Vector2(0.5f, 0.5f));
            return this;
        }
    }
}