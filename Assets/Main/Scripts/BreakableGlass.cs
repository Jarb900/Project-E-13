using UnityEngine;

namespace Main.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class BreakableGlass : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject brokenGlassVisuals; // Optional: Drag a "Broken Shards" model here
        public AudioClip breakSound;          // Drag a "Glass Shatter" sound here
        public float explosionForce = 200f; // How hard pieces fly out

        // Optional: Do you need a hammer to break it?
        public bool requiresHardObject = false;

        private AudioSource myAudio;

        private void Start()
        {
            myAudio = GetComponent<AudioSource>();
            if (brokenGlassVisuals) brokenGlassVisuals.SetActive(false);
        }

        public void Smash()
        {
            // 1. Play Sound
            // We use PlayClipAtPoint because this object is about to be disabled!
            if (breakSound) AudioSource.PlayClipAtPoint(breakSound, transform.position);

            // 2. Show broken pieces (if you have them)
            if (brokenGlassVisuals)
            {
                brokenGlassVisuals.SetActive(true);
                brokenGlassVisuals.transform.SetParent(null); // Detach so it doesn't vanish

                // 3. Add Explosion Physics
                Rigidbody[] shards = brokenGlassVisuals.GetComponentsInChildren<Rigidbody>();
                foreach (var rb in shards)
                {
                    // Push pieces outwards from the center
                    rb.AddExplosionForce(explosionForce, transform.position, 3f);
                }
            }

            // 4. Disable the solid glass
            // This is CRITICAL: Disabling this collider lets the player's Raycast hit the Axe inside!
            gameObject.SetActive(false);
        }
    }
}