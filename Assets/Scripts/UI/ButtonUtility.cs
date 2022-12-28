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
        VehicleSaver.GenerateVehicle(
        VehicleSaver.LoadVehicle("test"),
        FindObjectsOfType<Part>().ToList().Where(_ => _.isRoot).First().transform.parent);
        InputManager.SetGameCameraTarget(FindObjectsOfType<Part>().ToList().Where(_ => _.isRoot).First().transform);
    }
}