using UnityEngine;

namespace VehicularVanguard.Vehicle.Saving
{
    [System.Serializable]
    public class Version
    {
        [SerializeField]
        private string _data;
        public Version(string data) => _data = data;
        public bool Compatible(Version o)
        {
            //Todo implement complex version compatibility system
            return _data == o._data;
        }
    }
}