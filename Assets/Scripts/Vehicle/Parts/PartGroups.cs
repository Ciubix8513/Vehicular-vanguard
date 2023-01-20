using UnityEngine;

namespace CarGame.Vehicle
{
    public class PartGroups : MonoBehaviour
    {
        public GroupWrapper DownGroup;
        public GroupWrapper UpGroup;
        public static PartGroups Instance;
        public GroupWrapper this[int i]
        {
            get
            {
                if (i == 0)
                    return DownGroup;
                else
                    return UpGroup;
            }
        }
        void Awake()
        {
            Instance = this;
            DownGroup = new();
            UpGroup = new();
        }
    }
}