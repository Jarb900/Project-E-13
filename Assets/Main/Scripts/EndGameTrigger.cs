using UnityEngine;

namespace Main.Scripts
{
    public class EndGameTrigger : MonoBehaviour
    {
        public LevelLoader levelLoader; // Reference to the script above
        private bool hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;

            if (other.CompareTag("Player"))
            {
                hasTriggered = true;

                // Disable player movement so they don't walk around in the dark
                var controller = other.GetComponent<PlayerController>(); // Or whatever your movement script is called
                if (controller) controller.enabled = false;

                levelLoader.LoadNextLevel();
            }
        }
    }
}