using UnityEngine;

namespace Main.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class ProjectorLogic : MonoBehaviour
    {
        [Header("Visuals")]
        public GameObject projectorLight; // Drag the Purple Spotlight here
        public GameObject normalText;     // The text visible normally (e.g., "Class Schedule")
        public GameObject secretText;     // The hidden code (e.g., "472")

        [Header("Audio")]
        public AudioClip clickSound;      // The button click
        public AudioClip fanSound;        // The humming sound (Set this clip to Loop in project files!)

        public bool isOn = false;
        private AudioSource myAudio;

        private void Start()
        {
            myAudio = GetComponent<AudioSource>();

            // Ensure correct start state (Off)
            UpdateState();
        }

        public void Toggle()
        {
            isOn = !isOn; // Flip the switch
            UpdateState();
        }

        private void UpdateState()
        {
            // 1. Swap the Visuals
            if (projectorLight) projectorLight.SetActive(isOn);
            if (normalText) normalText.SetActive(!isOn); // Visible when OFF
            if (secretText) secretText.SetActive(isOn);  // Visible when ON

            // 2. Handle Audio
            if (isOn)
            {
                // Turn On: Click + Start Fan Loop
                if (clickSound) myAudio.PlayOneShot(clickSound);

                if (fanSound)
                {
                    myAudio.clip = fanSound;
                    myAudio.loop = true;
                    myAudio.PlayDelayed(0.2f); // Slight delay so click is heard first
                }
            }
            else
            {
                // Turn Off: Click + Stop Fan
                if (clickSound) myAudio.PlayOneShot(clickSound);
                myAudio.Stop();
            }
        }
    }
}