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

        public bool isOpen = false;

        // This is now called by PlayerInteraction
        // ReSharper disable Unity.PerformanceAnalysis
        public void TryUnlock(PlayerInventory inventory)
        {
            if (isOpen) return;

            // Check if the player's inventory list "Contains" our required key
            if (inventory.HasItem(requiredKey))
            {
                isOpen = true;
            
                if (objectAnimator)
                {
                    objectAnimator.SetTrigger(Open);
                }
            }
        }
    }
}