using System.Linq;
using UnityEngine;
using CarGame.Vehicle.Saving;

public class ButtonUtility : MonoBehaviour
{
    public void Load(int id) => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    public void Quit() => Application.Quit();
    public void TestSaving() => VehicleSaver.SaveVehicle(
        VehicleSaver.SerializeVehicle(FindObjectsOfType<Part>().ToList().Where(_ => _.isRoot).First()),
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
        CarGame.Vehicle.Editor.HistoryManager.Root = Root;
        CarGame.Vehicle.Editor.HistoryManager.ResetHistory();
    }
}