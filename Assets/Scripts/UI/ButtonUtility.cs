using System.Linq;
using UnityEngine;
using CarGame.Vehicle.Saving;
using CarGame.Vehicle;
using CarGame.Player;

namespace CarGame.UI.Utils
{
    public class ButtonUtility : MonoBehaviour
    {
        public void Load(int id) => UnityEngine.SceneManagement.SceneManager.LoadScene(id);
        public void Quit() => Application.Quit();
        public void TestSaving() => VehicleSaver.SaveVehicle(
            VehicleSaver.SerializeVehicle(FindObjectsOfType<Part>().ToList().First(_ => _.isRoot)),
            "test");
        public void TestLoading()
        {
            var Root = VehicleSaver.GenerateVehicle(
            VehicleSaver.LoadVehicle("test"),
            FindObjectsOfType<Part>().ToList().Where(_ => _.isRoot).First().transform.parent,
            null,
            out var n);
            // InputManager.SetGameCameraTarget(FindObjectsOfType<Part>().ToList().Where(_ => _.isRoot).First().transform);
            InputManager.SetGameCameraTarget(Root.transform);
            Vehicle.Editor.HistoryManager.Root = Root;
            Vehicle.Editor.HistoryManager.ResetHistory();
        }
    }
}