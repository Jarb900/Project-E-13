using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Main.Scripts
{
    public class RainZone : MonoBehaviour
    {
        [Header("Settings")]
        public AudioMixer rainMixer;        // Drag your RainMixer here
        public string parameterName = "RainVol"; // Must match the exposed name
        public float fadeTime = 2.0f;       // How long the fade takes (seconds)
        public float rainVolumeOn = -20f;   // Volume when inside the room (dB)
        public float rainVolumeOff = -80f;  // Volume when leaving the room (dB)

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
                // This name must match the IEnumerator below
                StartCoroutine(FadeMixer(rainVolumeOn)); 
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
                // This name must match the IEnumerator below
                StartCoroutine(FadeMixer(rainVolumeOff)); 
            }
        }

        // FIXED: Renamed this from 'Fade' to 'FadeMixer'
        IEnumerator FadeMixer(float targetVolume)
        {
            float currentVolume;
            bool result = rainMixer.GetFloat(parameterName, out currentVolume);
        
            // clear error if you forgot to expose the parameter
            if (!result)
            {
                Debug.LogError("Could not find parameter: " + parameterName + ". Make sure you exposed it in the Audio Mixer!");
                yield break;
            }

            float currentTime = 0;

            while (currentTime < fadeTime)
            {
                currentTime += Time.deltaTime;
                // Smoothly move from current volume to target volume
                float newVol = Mathf.Lerp(currentVolume, targetVolume, currentTime / fadeTime);
                rainMixer.SetFloat(parameterName, newVol);
                yield return null;
            }
            // Ensure we hit the exact target at the end
            rainMixer.SetFloat(parameterName, targetVolume);
        }
    }
}