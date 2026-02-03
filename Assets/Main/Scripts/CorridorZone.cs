using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Main.Scripts
{
    public class CorridorZone : MonoBehaviour
    {
        [Header("Settings")]
        [Header("Layer 1: Ambience")]
        public AudioSource ambienceSource;
        [Range(0f, 1f)] public float ambienceMaxVol = 1.0f;

        [Header("Layer 2: Music")]
        public AudioSource musicSource;
        [Range(0f, 1f)] public float musicMaxVol = 0.5f;
        public float musicDelay = 5.0f;

        [Header("Settings")]
        public float fadeTime = 2.0f;

        private Coroutine currentFade;

        private void Start()
        {
            // Ensure both start silent
            if (ambienceSource) ambienceSource.volume = 0;
            if (musicSource) musicSource.volume = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (currentFade != null) StopCoroutine(currentFade);
                currentFade = StartCoroutine(EnterRoomSequence());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (currentFade != null) StopCoroutine(currentFade);
                currentFade = StartCoroutine(ExitRoomSequence());
            }
        }

        IEnumerator EnterRoomSequence()
        {
            // 1. Start Ambience IMMEDIATELY
            if (ambienceSource)
            {
                if (!ambienceSource.isPlaying) ambienceSource.Play();
                StartCoroutine(FadeSource(ambienceSource, ambienceMaxVol, fadeTime));
            }

            // 2. WAIT for the delay
            if (musicSource)
            {
                yield return new WaitForSeconds(musicDelay);

                // 3. Start Music AFTER the delay
                if (!musicSource.isPlaying) musicSource.Play();
                StartCoroutine(FadeSource(musicSource, musicMaxVol, fadeTime));
            }
        }

        IEnumerator ExitRoomSequence()
        {
            // Fade BOTH out immediately when leaving (no delay needed to stop)
            if (ambienceSource) StartCoroutine(FadeSource(ambienceSource, 0f, fadeTime));
            if (musicSource) StartCoroutine(FadeSource(musicSource, 0f, fadeTime));
            yield break;
        }

        // Helper to fade any audio source
        IEnumerator FadeSource(AudioSource source, float targetVol, float duration)
        {
            float startVol = source.volume;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                source.volume = Mathf.Lerp(startVol, targetVol, timer / duration);
                yield return null;
            }
            source.volume = targetVol;
        }
    }
}