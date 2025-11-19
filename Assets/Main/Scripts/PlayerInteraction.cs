using ElmanGameDevTools.FirstPersonControllerPro.Scripts.PlayerSystem;
using UnityEngine;
using TMPro;

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
        
        [Header("UI Feedback")] // <-- NEW SECTION
        public GameObject interactionCanvas; // Drag the Canvas_Interaction here
        public GameObject Cursor; // Drag the Cursor Prefab here
        public GameObject LockedDoor;
        public GameObject UnlockDoor;
        private bool isUiVisible = false; // Internal flag
        private bool playerHasKey = false;
        private bool lookingAtDoor = false;
        
        private void Start()
        {
            inventory = GetComponent<PlayerInventory>();
        }

        private void Update()
        {
            // --- 1. INITIAL CHECKS AND RAYCAST (Runs Every Frame) ---
            if (!playerCamera) return;
    
            // Create and draw the ray
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit = new RaycastHit(); // Initialize hit to avoid local variable errors
            bool lookingAtInteractable = false;
    
            // Perform the raycast and ensure ALL logic that uses 'hit' is inside this block
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                // Check if the object we hit is tagged 'Interactable' for UI feedback
                if (hit.collider.CompareTag("Interactable")) 
                {
                    lookingAtInteractable = true;
                }
            }
    
            // --- 2. UI VISIBILITY LOGIC (Runs every frame, regardless of hit) ---
            if (lookingAtInteractable != isUiVisible)
            {
                interactionCanvas.SetActive(lookingAtInteractable);
                Cursor.SetActive(!lookingAtInteractable);
                isUiVisible = lookingAtInteractable;
            }
    
            // --- 3. HANDLE KEY PRESS (ACTION LOGIC) ---
            // Guard clause 1: Stop if key is not pressed
            if (!Input.GetKeyDown(interactionKey)) return;

            // Guard clause 2: Stop if the ray didn't hit anything in the last frame
            // This is the CRITICAL line that prevents the NullReferenceException
            if (hit.collider == null) return; 

            // --- 4. CHECK OBJECT TYPE AND INTERACT ---

            // Check for Safe Keypad (Highest Priority)
            SafeInteract safe = hit.collider.GetComponent<SafeInteract>();
            if (safe != null)
            {
                safe.ShowKeypad(GetComponent<PlayerController>());
                return;
            }

            // Check for Item Pickup
            var pickup = hit.collider.GetComponent<ItemPickup>();
            if (pickup)
            {
                pickup.Pickup(inventory);
                return;
            }
    
            // Check for KeyLock
            var lockScript = hit.collider.GetComponent<KeyLock>();
            if (lockScript)
            {
                lockScript.TryUnlock(inventory);
                return;
            }
        }
    }
}
