using UnityEngine;

namespace CarGame.Vehicle.Saving
{
    [System.Serializable]
    public struct TransformData
    {
        public Vector3 Position;
        public Vector3 RotationEuler;
        public Vector3 Scale;

        public TransformData(Transform t)
        {
            Position = t.localPosition;
            RotationEuler = t.localEulerAngles;
            Scale = t.localScale;
        }
    }
}