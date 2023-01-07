using UnityEngine;

namespace CarGame.Vehicle
{
    public class PartProxy : MonoBehaviour
    {
        public Part part;
        public bool Activatable { get => part.Activatable; }
    }
}