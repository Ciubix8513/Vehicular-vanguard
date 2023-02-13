using VehicularVanguard.Player;

namespace VehicularVanguard.Vehicle.Saving
{
    [System.Serializable]
    public class HistoryVehicle
    {
        public Vehicle Vehicle;
        public EditorMode EditorMode;
        public int SelectedPartId;

        public HistoryVehicle(Vehicle vehicle, EditorMode editorMode, int selectedPartId)
        {
            Vehicle = vehicle;
            EditorMode = editorMode;
            SelectedPartId = selectedPartId;
        }
    }
}