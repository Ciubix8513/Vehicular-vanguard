using UnityEngine;

namespace VehicularVanguard.Vehicle
{
    public class PartProxy : MonoBehaviour
    {
        public Part part;
        public bool Activatable { get => part.Activatable; }
    }
}