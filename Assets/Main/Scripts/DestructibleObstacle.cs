using UnityEngine;

namespace Main.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class DestructibleObstacle : MonoBehaviour
    {
        [Header("Requirements")]
        public ItemData requiredTool; // Drag your 'Axe' ItemData here

        [Header("Feedback")]
        public AudioClip destroySound; // Drag 'Wood Break' sound here
        public GameObject destroyParticles; // Optional wood chips
        public float explosionForce = 300f;

        private AudioSource myAudio;

        private void Start()
        {
            myAudio = GetComponent<AudioSource>();
        }

        public void TryDestroy(PlayerInventory inventory)
        {
            // 1. Check for Axe
            if (inventory.HasItem(requiredTool))
            {
                // Success!
                if (destroySound) AudioSource.PlayClipAtPoint(destroySound, transform.position);
              

                // 2. Spawn the Debris (The "Fake" broken pieces)
                if (destroyParticles)
                {
                    GameObject debris = Instantiate(destroyParticles, transform.position, transform.rotation);
                    debris.SetActive(true); // Make sure it's visible

                    // 3. Apply Explosion Force to the new pieces
                    Rigidbody[] rbs = debris.GetComponentsInChildren<Rigidbody>();
                    foreach (var rb in rbs)
                    {
                        rb.AddExplosionForce(explosionForce, transform.position, 3f);
                    }

                    // 4. Cleanup debris after 5 seconds (Optional, for performance)
                    Destroy(debris, 5f);
                }

                // 5. Delete the original Blockage
                Destroy(gameObject);
            }
            else
            {
                
            }
        }
    }
}