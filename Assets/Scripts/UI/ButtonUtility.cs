using UnityEngine;

namespace CarGame.UI.Utils
{
    public class ButtonUtility : MonoBehaviour
    {
        public void Load(int id) => UnityEngine.SceneManagement.SceneManager.LoadScene(id);
        public void Quit() => Application.Quit();
    }
}