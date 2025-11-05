using UnityEngine;

namespace Main.Scripts
{
    public class KeyLock : MonoBehaviour
    {
        private static readonly int Open = Animator.StringToHash("Open");

        [Header("Animation")]
        public Animator objectAnimator;

        [Header("Settings")]
        [Tooltip("The specific ItemData for the key that opens this lock.")]
        public ItemData requiredKey; // Drag your 'Key_Door' asset here

        private bool isLocked = true;

        // This is now called by PlayerInteraction
        // ReSharper disable Unity.PerformanceAnalysis
        public void TryUnlock(PlayerInventory inventory)
        {
            if (!isLocked) return;

            // Check if the player's inventory list "Contains" our required key
            if (inventory.HasItem(requiredKey))
            {
                Debug.Log("Key fits! Opening door.");
                isLocked = false;
            
                if (objectAnimator)
                {
                    objectAnimator.SetTrigger(Open);
                }

                var col = GetComponent<Collider>();
                if (col) col.enabled = false;
            }
            else
            {
                Debug.Log("This is locked. I need the " + requiredKey.itemName);
            }
        }
    }
}