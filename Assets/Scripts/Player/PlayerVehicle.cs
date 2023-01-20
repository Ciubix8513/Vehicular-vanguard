using System.Linq;
using CarGame.Vehicle;
using CarGame.Vehicle.Saving;
using UnityEngine;

namespace CarGame.Player
{
    public class PlayerVehicle : MonoBehaviour
    {
        public Part Root;
        private TransformData _initialPosition;
        void Awake() => _initialPosition = new TransformData(transform);

        public void ResetTransform()
        {
            transform.Load(_initialPosition);
            GetComponentsInChildren<Rigidbody>().ToList().ForEach(_ =>
            {
                _.velocity = Vector3.zero;
                _.angularVelocity = Vector3.zero;
            });
        }
    }
}