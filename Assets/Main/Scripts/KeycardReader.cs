using UnityEngine;

namespace Main.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class KeycardReader : MonoBehaviour
    {
        [Header("Security Settings")]
        public ItemData requiredCard;      // Drag your 'Blue Keycard' ItemData here
        public Animator doorAnimator;      // Drag the Door's Animator here
        public string openTrigger = "Open"; // The name of the parameter in the Animator

        [Header("Power Settings")] // <--- NEW SECTION
        public FuseBoxPart powerSource; // Drag the Fuse Box Switch here
        public AudioClip noPowerSound; // Optional: A "Dead Click" sound

        [Header("Visual Feedback")]
        public GameObject statusLight;     // Drag the Point Light on the reader
        public Color lockedColor = Color.red;
        public Color unlockedColor = Color.green;
        public Color offColor = Color.black; // The color when it "dies"

        [Header("Audio Feedback")]
        public AudioClip successSound;     // "Beep"
        public AudioClip deniedSound;      // "Buzz/Error"

        public bool isUnlocked = false;
        private AudioSource myAudio;
        private Light lightComponent;

        private void Start()
        {
            myAudio = GetComponent<AudioSource>();

            if (statusLight)
            {
                lightComponent = statusLight.GetComponent<Light>();

                SetLightColor(lockedColor);
            }
        }
        private void Update()
        {
            // CONSTANTLY CHECK: Did the player turn the power back on?
            if (powerSource != null && powerSource.SwitchHasBeenUsed)
            {
                // If power is back, but light is "dead" (black), bring it back to life!
                if (lightComponent.color == offColor && !isUnlocked)
                {
                    SetLightColor(lockedColor); // Restore Red Light
                }
            }
        }
        public void TryAccess(PlayerInventory inventory)
        {
            // 1. Check Power First!
            if (powerSource != null && !powerSource.SwitchHasBeenUsed)
            {
                SetLightColor(offColor);

                // Power is OFF
                if (noPowerSound) myAudio.PlayOneShot(noPowerSound);
                return; // Stop here, don't check for cards
            }

            if (isUnlocked) return;

            // 2. Check Inventory
            if (inventory.HasItem(requiredCard))
            {
                GrantAccess();
            }
            else
            {
                DenyAccess();
            }
        }

        // Helper to change both the Light Glow and the Mesh Material
        private void SetLightColor(Color col)
        {
            if (lightComponent)
            {
                lightComponent.color = col;
                // If turning off, disable the light component entirely to save performance
                lightComponent.enabled = (col != offColor);
            }

        }
        private void GrantAccess()
        {
            isUnlocked = true;

            // Visuals: Turn Green
            if (lightComponent) lightComponent.color = unlockedColor;

            // Audio: Beep
            if (successSound) myAudio.PlayOneShot(successSound);

            // Action: Open the Door
            if (doorAnimator) doorAnimator.SetTrigger(openTrigger);
        }

        private void DenyAccess()
        {
            // Audio: Error Buzz
            if (deniedSound) myAudio.PlayOneShot(deniedSound);

        }
    }
}