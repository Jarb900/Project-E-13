using UnityEngine;

namespace Main.Scripts
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction")]
        public float interactionDistance = 3f; // How far the player can interact
        public KeyCode interactionKey = KeyCode.E; // The key to interact
        public Transform playerCamera; // Drag your Main Camera here
        private PlayerInventory inventory; // Reference to the PlayerInventory script

        private void Start()
        {
            inventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            if (!playerCamera) return;
            Debug.DrawRay(playerCamera.position, playerCamera.forward * interactionDistance, Color.blue);
            
            // 2. This handles the "interaction"
            if (!Input.GetKeyDown(interactionKey)) return;
            // Shoot a ray from the camera
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            // If the ray hits something within interactionDistance
            if (!Physics.Raycast(ray, out hit, interactionDistance)) return;
            // Check for our new "ItemPickup" script
            var pickup = hit.collider.GetComponent<ItemPickup>();
            if (pickup)
            {
                // Tell the pickup to add itself to our inventory
                pickup.Pickup(inventory);
                return;
            }

            // Check for our "KeyLock" script
            var lockScript = hit.collider.GetComponent<KeyLock>();
            if (!lockScript) return;
            lockScript.TryUnlock(inventory); // Pass in the whole inventory
            return;
        }
    }
}
