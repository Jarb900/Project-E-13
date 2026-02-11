using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scripts
{
    public class MenuMusic : MonoBehaviour
    {
        private static MenuMusic instance;

        void Awake()
        {
            // SINGLETON PATTERN:
            // This ensures we don't get 2 songs playing on top of each other
            // if we go from Menu -> Game -> Back to Menu.
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject); // This command keeps the music alive!
        }

        void OnEnable()
        {
            // Listen for when a new scene loads
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            // Stop listening (cleanup)
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // CHECK: Is this the game scene?
            // If we loaded the "Bedroom" or "MainScene", stop the menu music.
            // CHANGE "MainScene" to match your actual game scene name!
            if (scene.name == "MainSceneFinal" || scene.name == "Bedroom")
            {
                Destroy(gameObject); // Destroys the music object so game audio can take over
            }
        }
    }
}