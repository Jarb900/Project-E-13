using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Main.Scripts
{
    public class LevelLoader : MonoBehaviour
    {
        [Header("Settings")]
        public CanvasGroup blackScreen; // Drag your Black Panel here
        public float fadeDuration = 2.0f;
        public string nextSceneName = "PlayerRoom"; // Exact name of your next scene

        private void Start()
        {
            // Optional: Fade IN when this level starts
            if (blackScreen != null)
            {
                blackScreen.alpha = 1;
                StartCoroutine(Fade(0)); // Fade to transparent
            }
        }

        public void LoadNextLevel()
        {
            StartCoroutine(FadeAndLoad());
        }

        private IEnumerator FadeAndLoad()
        {
            // 1. Fade OUT (To Black)
            yield return StartCoroutine(Fade(1));

            // 2. Wait a tiny moment for drama
            yield return new WaitForSeconds(0.5f);

            // 3. Load the Scene
            SceneManager.LoadScene(nextSceneName);
        }

        private IEnumerator Fade(float targetAlpha)
        {
            float startAlpha = blackScreen.alpha;
            float time = 0;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
                blackScreen.alpha = newAlpha;
                yield return null;
            }
            blackScreen.alpha = targetAlpha;
        }
    }
}