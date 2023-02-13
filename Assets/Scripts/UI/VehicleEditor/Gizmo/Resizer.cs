using UnityEngine;
namespace VehicularVanguard.Vehicle.Editor.Utils
{
    public class Resizer : MonoBehaviour
    {
        Camera _camera;
        public float ConstSize = .2f;
        void Awake() => _camera = Camera.main;
        void Update()
        {
            float mult = (transform.position - _camera.transform.position).magnitude;
            transform.localScale = new Vector3(ConstSize, ConstSize, ConstSize) * mult;
        }
    }
}