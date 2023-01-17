using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

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
        public Saving.Vehicle vehicle;
        LoadDialogue parent;
        public void Delete()
        {
            File.Delete(vehicle.FileName);
            parent.LoadSaves();
        }
        public LoadingMenuItem Init(Saving.Vehicle v, LoadDialogue par)
        {
            Texture2D tex = new(1, 1);
            tex.LoadImage(v.PreviewImage);
            _name.text = v.Name;
            _date.text = "Created: " + v.CreationDate;
            _image.sprite = Sprite.Create(tex,
                                          new Rect(0, 0, tex.width, tex.height),
                                          new Vector2(0.5f, 0.5f));
            vehicle = v;
            //Remove what I don't need anymore
            v.PreviewImage = new byte[0];
            parent = par;
            return this;
        }
    }
}