using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("UI Reference")]
        public GameObject pauseCanvas; // Drag your Canvas_Pause here

        [Header("Player Reference")]
        // Drag your Player object (the one with the PlayerController script) here!
        public PlayerController playerScript;

        private bool isPaused = false;

        private void Update()
        {
            // Toggle Pause when ESC is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            // 1. Hide the Menu
            pauseCanvas.SetActive(false);

            // 2. Un-freeze time
            Time.timeScale = 1f;

            // 3. Lock the cursor back to the game
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (playerScript != null)
            {
                playerScript.enabled = true;
            }

            isPaused = false;
        }

        public void PauseGame()
        {
            // 1. Show the Menu
            pauseCanvas.SetActive(true);

            // 2. Freeze time (Physics and Animation stop)
            Time.timeScale = 0f;

            // 3. Unlock the cursor so you can click buttons
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (playerScript != null)
            {
                playerScript.enabled = false;
            }

            isPaused = true;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}