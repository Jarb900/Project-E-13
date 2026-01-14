using System.Collections.Generic;
using UnityEngine;

// Required for Lists

namespace Main.Scripts
{
    public class RandomSoundPlayer : MonoBehaviour
    {
        [Header("Audio Setup")]
        public AudioSource audioSource;
        public List<AudioClip> soundClips; // The list of sounds to pick from

        [Header("Timing Settings")]
        public float minWaitTime = 10f;   // Minimum time between sounds
        public float maxWaitTime = 30f;   // Maximum time between sounds

        [Header("Variation")]
        [Range(0f, 1f)] public float volumeBase = 0.5f; // Base volume
        [Range(0f, 0.2f)] public float volumeRandom = 0.1f; // Random +/- volume

        private void Start()
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            StartCoroutine(PlayRandomSounds());
        }

        private System.Collections.IEnumerator PlayRandomSounds()
        {
            while (true)
            {
                // 1. Wait for a random amount of time
                var waitTime = Random.Range(minWaitTime, maxWaitTime);
                yield return new WaitForSeconds(waitTime);

                // 2. Pick a random sound from the list
                if (soundClips.Count <= 0) continue;
                var clipToPlay = soundClips[Random.Range(0, soundClips.Count)];

                // 3. Randomize volume slightly for realism
                var randomVol = volumeBase + Random.Range(-volumeRandom, volumeRandom);
                
                // 4. Play it once (OneShot allows sounds to overlap if needed)
                audioSource.PlayOneShot(clipToPlay, randomVol);
            }
        }
    }
}