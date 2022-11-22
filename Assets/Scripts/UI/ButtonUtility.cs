using UnityEngine;
public class ButtonUtility : MonoBehaviour
{
    public void Load(int id) =>UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    public void Quit() => Application.Quit();
}