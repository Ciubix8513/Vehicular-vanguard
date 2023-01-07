using UnityEngine;

namespace CarGame.Vehicle
{
    public class PartGroups : MonoBehaviour
    {
        public static GroupWrapper DownGroup;
        public static GroupWrapper UpGroup;
        void Awake()
        {
            DownGroup = new();
            UpGroup = new();
        }
    }
}