using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Scene Selection")]
        public string firstLevelName = "MainScene"; // <-- Type the EXACT name of your game scene here!

        private void Start()
        {
            // CRITICAL: Ensure the cursor is visible when we load the menu
            // (Otherwise, if you come from gameplay, the cursor might still be hidden)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(firstLevelName);
        }

        public void QuitGame()
        {
            Debug.Log("Quitting Game...");
            Application.Quit();
        }
    }
}