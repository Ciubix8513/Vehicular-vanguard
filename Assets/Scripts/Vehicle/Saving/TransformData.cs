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
    //An extension class and an extension function just to make my life a bit easier
    public static class TransformDataTransformExtension
    {
        public static void Load(this Transform t, TransformData d)
        {
            t.localPosition = d.Position;
            t.localEulerAngles = d.RotationEuler;
            t.localScale = d.Scale;
        }
    }
}