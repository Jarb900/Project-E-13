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

        [Header("Door UI")]
        public GameObject LockedDoor;
        public GameObject UnlockDoor;
        public ItemData requiredKey;

        [Header("Fuse Box UI")] // <--- NEW VARIABLES
        public GameObject FuseOpenUI; // Drag your "Press E to Open" text here
        public GameObject FuseFlipUI; // Drag your "Press E to Flip Switch" text here

        [Header("Projector UI")] // <--- NEW VARIABLES
        public GameObject ProjectorUI; // Drag your "Press E to Open" text here

        private bool isUiVisible = false; // Internal flag
        private bool playerHasKey = false;

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
            bool lookingAtDoor = false;
            bool lookingAtProjector = false;
            FuseBoxPart currentFusePart = null;
            ProjectorLogic currentProjector = null;

            // Perform the raycast and ensure ALL logic that uses 'hit' is inside this block
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                Debug.Log("I hit: " + hit.collider.name);
                // Check if the object we hit is tagged 'Interactable' for UI feedback
                if (hit.collider.CompareTag("Interactable")) 
                {
                    lookingAtInteractable = true;
                }

                if (hit.collider.CompareTag("Door"))
                {
                    lookingAtDoor = true;
                }

                if (hit.collider.CompareTag("Projector"))
                {
                    lookingAtProjector = true;
                }
                currentFusePart = hit.collider.GetComponent<FuseBoxPart>();
                currentProjector = hit.collider.GetComponent<ProjectorLogic>();
            }

            // --- 2. UI VISIBILITY LOGIC (Runs every frame, regardless of hit) ---
            if (lookingAtInteractable != isUiVisible)
            { 
                interactionCanvas.SetActive(lookingAtInteractable); 
                Cursor.SetActive(!lookingAtInteractable); 
                isUiVisible = lookingAtInteractable; 
            }

            if (lookingAtDoor)
            {
                // Try to read door's isOpen property
                var door = hit.collider.GetComponent<KeyLock>();

                if (door != null && door.isOpen)
                {
                    // Door is already open → hide UI
                    LockedDoor.SetActive(false);
                    UnlockDoor.SetActive(false);
                }
                else
                {
                    // Door is closed → check for key
                    playerHasKey = inventory.HasItem(requiredKey);

                    LockedDoor.SetActive(!playerHasKey);
                    UnlockDoor.SetActive(playerHasKey);
                }
            }
            else
            {
                LockedDoor.SetActive(false);
                UnlockDoor.SetActive(false);
            }

            // Fuse Box UI Logic
            if (currentFusePart != null)
            {
                // CASE 1: Looking at the Door
                if (currentFusePart.triggerName == "TrigOpen")
                {
                    // Show "Open UI" only if door is CLOSED
                    FuseOpenUI.SetActive(!currentFusePart.isDoorOpen);
                    FuseFlipUI.SetActive(false);
                }
                // CASE 2: Looking at the Switch
                else if (currentFusePart.triggerName == "TrigSwitch")
                {
                    // Show "Flip UI" only if NOT used yet
                    FuseFlipUI.SetActive(!currentFusePart.SwitchHasBeenUsed);
                    FuseOpenUI.SetActive(false);
                }
            }
            else
            {
                // We aren't looking at any fuse part, hide both
                FuseOpenUI.SetActive(false);
                FuseFlipUI.SetActive(false);
            }

            // Projector UI Logic
            if (currentProjector != null)
            {
                ProjectorUI.SetActive(!currentProjector.isOn);
            }
            else
            {
                // Turns off Projector UI
                ProjectorUI.SetActive(false);

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

            // [NEW] Check for Fuse Box parts (Switch or Door)
            var fusePart = hit.collider.GetComponent<FuseBoxPart>();
            if (fusePart != null)
            {
                fusePart.Interact();
                return;
            }

            // Check for Projector
            var projector = hit.collider.GetComponent<ProjectorLogic>();
            if (projector != null)
            {
                projector.Toggle();
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
