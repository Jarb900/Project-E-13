using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts
{
    public class EndGame : MonoBehaviour
    {
        public string menuSceneName = "MainMenu"; // Make sure this matches your scene name exactly

        public void LoadMenu()
        {
            Debug.Log("Cutscene Over. Loading Menu...");

            // 1. Unlock the Cursor (CRITICAL!)
            // If we don't do this, the cursor stays hidden from the FPS controller
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 2. Load the Menu
            SceneManager.LoadScene(menuSceneName);
        }
    }
}