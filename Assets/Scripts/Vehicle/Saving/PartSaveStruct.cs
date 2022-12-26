namespace CarGame.Vehicle.Saving
{
    [System.Serializable]
    public struct PartSaveStruct
    {
        public PartSaveStruct(int saveID, TransformData transform, string iD, int attachedPartID)
        {
            SaveID = saveID;
            Transform = transform;
            ID = iD;
            AttachedPartID = attachedPartID;
        }

        public int SaveID;
        public TransformData Transform;
        public string ID;
        public int AttachedPartID;
    }
}