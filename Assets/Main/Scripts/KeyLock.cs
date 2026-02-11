using UnityEngine;

namespace Main.Scripts
{
    public class KeyLock : MonoBehaviour
    {
        [Header("Door Settings")]
        public bool startOpen = false; // <--- NEW CHECKBOX
        public bool isLocked = true;
        public bool isOpen = false;

        [Header("Animation")]
        public Animator doorAnimator;
        public string openBoolName = "Open";
        public string openStateName = "DoorForGiants";


        [Header("Audio")]
        public AudioClip slamSound;
        public AudioClip openSound;
        public AudioClip lockedSound;
        private AudioSource myAudio;

        [Header("Items")]
        [Tooltip("The specific ItemData for the key that opens this lock.")]
        public ItemData requiredKey; // Drag your 'Key_Door' asset here

        private void Start()
        {
            myAudio = GetComponent<AudioSource>();

            if (startOpen)
            {
                // 1. Force internal state to Open
                isOpen = true;
                isLocked = false;

                // 2. Force Animator to Open State immediately
                if (doorAnimator)
                {
                    doorAnimator.SetBool(openBoolName, true);

                    // TRICK: Skip the "Opening" animation and jump to the end (1.0f)
                    // This prevents the door from "swinging open" when the scene loads.
                    doorAnimator.Play(openStateName, 0, 1.0f);
                }
            }
        }
        // This is now called by PlayerInteraction
        // ReSharper disable Unity.PerformanceAnalysis
        public void TryUnlock(PlayerInventory inventory)
        {
            if (isOpen) return;

            if (!inventory.HasItem(requiredKey))
            {
                if (openSound && myAudio) myAudio.PlayOneShot(lockedSound);
            }

            // Check if the player's inventory list "Contains" our required key
            if (inventory.HasItem(requiredKey))
            {
                isOpen = true;

                if (doorAnimator)
                {
                    doorAnimator.SetTrigger("Open");
                }

                if (openSound && myAudio) myAudio.PlayOneShot(openSound);
            }
            
        }
        public void SlamShut()
        {
            if (!isOpen) return; // Already closed

            // 1. Close the door physically
            isOpen = false;
            if (doorAnimator) doorAnimator.SetBool(openBoolName, false);

            // 2. LOCK IT! (This is crucial)
            // This forces the player to need the key again.
            isLocked = true;

            // 3. Play Sound
            if (slamSound && myAudio) myAudio.PlayOneShot(slamSound);
        }

    }
}