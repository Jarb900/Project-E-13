using UnityEngine;

namespace Main.Scripts
{
    public class RoomTrap : MonoBehaviour
    {
        [Header("Trap Settings")]
        public KeyLock targetDoor; // Drag the Door object here
        public bool oneTimeOnly = true;

        private bool hasTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (oneTimeOnly && hasTriggered) return;

            // Check if it's the player
            if (other.CompareTag("Player"))
            {
                if (targetDoor != null)
                {
                    targetDoor.SlamShut(); // Call the function we just made
                    hasTriggered = true;

                    // Optional: Disable this trigger so it doesn't happen again
                    if (oneTimeOnly) gameObject.SetActive(false);
                }
            }
        }
    }
}