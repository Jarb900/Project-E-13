using UnityEngine;

namespace Main.Scripts
{
    public class NaturalAmbience : MonoBehaviour
    {
        [Header("Audio Setup")]
        public AudioSource audioSource;

        [Header("Randomness Settings")]
        [Tooltip("Lower pitch = slower/deeper crickets. Higher = faster.")]
        public float minPitch = 0.9f;
        public float maxPitch = 1.1f;
    
        [Tooltip("How fast the pitch changes. Lower is more subtle.")]
        public float changeSpeed = 0.5f;

        private float randomOffset;

        private void Start()
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
        
            // Setup standard settings
            audioSource.loop = true;
            audioSource.Play();
        
            // Give a random starting point so it's different every time you play
            randomOffset = Random.Range(0f, 100f);
        }

        private void Update()
        {
            // 1. Calculate a smooth random number between 0 and 1
            // We use Time.time to move through the "noise" over time
            var noiseValue = Mathf.PerlinNoise((Time.time * changeSpeed) + randomOffset, 0f);

            // 2. Smoothly adjust the pitch based on that noise
            audioSource.pitch = Mathf.Lerp(minPitch, maxPitch, noiseValue);
        }
    }
}