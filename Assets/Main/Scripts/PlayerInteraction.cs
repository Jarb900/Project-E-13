using System.Collections; // Required for Coroutines
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

        [Header("Safe UI")]
        public GameObject SafeInteractUI;

        [Header("Door UI")]
        public GameObject LockedDoor;
        public GameObject UnlockDoor;
        public ItemData requiredKey;

        [Header("Fuse Box UI")] // <--- NEW VARIABLES
        public GameObject FuseOpenUI; // Drag your "Press E to Open" text here
        public GameObject FuseFlipUI; // Drag your "Press E to Flip Switch" text here

        [Header("Projector UI")] // <--- NEW VARIABLES
        public GameObject ProjectorUI; // Drag your "Press E to Open" text here

        [Header("Keycard Reader UI")]
        public GameObject ReaderLockedUI; // "Needs Keycard"
        public GameObject ReaderUnlockUI; // "Press E to Swipe"
        public GameObject ReaderNoPowerUI; // <--- NEW: "No Power"

        [Header("Destruction UI")] // <--- NEW SECTION
        public GameObject GlassBreakUI;      // "Press E to Smash"
        public GameObject ObstacleStuckUI;   // "It's Stuck / Need Axe"
        public GameObject ObstacleDestroyUI; // "Press E to Destroy"

        private bool isUiVisible = false; // Internal flag

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
            KeycardReader currentReader = null;       // <--- NEW
            BreakableGlass currentGlass = null;       // <--- NEW
            DestructibleObstacle currentObstacle = null;
            SafeInteract currentsafe = null;

            // Perform the raycast and ensure ALL logic that uses 'hit' is inside this block
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                //Debug.Log("I hit: " + hit.collider.name);
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
                currentReader = hit.collider.GetComponent<KeycardReader>();
                currentGlass = hit.collider.GetComponent<BreakableGlass>();
                currentObstacle = hit.collider.GetComponent<DestructibleObstacle>();
                currentsafe = hit.collider.GetComponent<SafeInteract>();
            }

            // --- 2. UI VISIBILITY LOGIC (Runs every frame, regardless of hit) ---
            if (lookingAtInteractable != isUiVisible)
            { 
                interactionCanvas.SetActive(lookingAtInteractable); 
                Cursor.SetActive(!lookingAtInteractable); 
                isUiVisible = lookingAtInteractable; 
            }

            LockedDoor.SetActive(false);
            UnlockDoor.SetActive(false);
            FuseOpenUI.SetActive(false);
            FuseFlipUI.SetActive(false);
            ProjectorUI.SetActive(false);
            ReaderLockedUI.SetActive(false);
            ReaderUnlockUI.SetActive(false);
            if (!ReaderNoPowerUI.activeSelf)
            {
                ReaderLockedUI.SetActive(false);
                ReaderUnlockUI.SetActive(false);
            }
            GlassBreakUI.SetActive(false);
            ObstacleStuckUI.SetActive(false);
            ObstacleDestroyUI.SetActive(false);
            SafeInteractUI.SetActive(false);

            if (lookingAtDoor)
            {
                // Try to read door's isOpen property
                var door = hit.collider.GetComponent<KeyLock>();

                if (door != null)
                {
                    if (door.isOpen)
                    {
                        // Door is already open → hide UI
                        LockedDoor.SetActive(false);
                        UnlockDoor.SetActive(false);
                    }
                    else
                    {
                        // 2. CHECK THE DOOR'S REQUIREMENT, NOT THE PLAYER'S VARIABLE
                        // 'door.requiredKey' accesses the specific ItemData slot on that specific door
                        bool hasTheRightKey = inventory.HasItem(door.requiredKey);

                        LockedDoor.SetActive(!hasTheRightKey); // Show "Locked" if we DON'T have it
                        UnlockDoor.SetActive(hasTheRightKey);  // Show "Unlock" if we DO have it
                    }
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

            // 4. [NEW] Keycard Reader Logic
            else if (currentReader != null)
            {
                // HERE IS THE FIX: We ONLY check inventory. We DO NOT check power here.
                // We pretend the reader is working fine until the player clicks it.
                // 1. If already unlocked, HIDE EVERYTHING
                if (currentReader.isUnlocked)
                {
                    ReaderLockedUI.SetActive(false);
                    ReaderUnlockUI.SetActive(false);
                    // (ReaderNoPowerUI is handled by the coroutine, so we leave it alone)
                }
                // 2. If locked, show the prompts
                else if (!ReaderNoPowerUI.activeSelf) 
                {
                    bool hasCard = inventory.HasItem(currentReader.requiredCard);
                    ReaderLockedUI.SetActive(!hasCard); 
                    ReaderUnlockUI.SetActive(hasCard);  
                }
            }

            // 5. [NEW] Breakable Glass Logic
            else if (currentGlass != null)
            {
                GlassBreakUI.SetActive(true); // Always show "Break" if looking at glass
            }

            // 6. [NEW] Destructible Obstacle Logic
            else if (currentObstacle != null)
            {
                // Check if we have the axe/tool
                bool hasTool = inventory.HasItem(currentObstacle.requiredTool);
                ObstacleStuckUI.SetActive(!hasTool);   // "It's Blocked"
                ObstacleDestroyUI.SetActive(hasTool);  // "Destroy"
            }

            else if (currentsafe != null)
            {
                SafeInteractUI.SetActive(true);
            }

            // --- 3. HANDLE KEY PRESS (ACTION LOGIC) ---
            // Guard clause 1: Stop if key is not pressed
            if (!Input.GetKeyDown(interactionKey)) return;

            // Guard clause 2: Stop if the ray didn't hit anything in the last frame
            // This is the CRITICAL line that prevents the NullReferenceException
            if (hit.collider == null) return;

            // [NEW] Keycard Interaction with Surprise Check
            if (currentReader != null)
            {
                // Check if power is OFF
                if (currentReader.powerSource != null && !currentReader.powerSource.SwitchHasBeenUsed)
                {
                    // POWER IS OFF! Trigger the surprise.
                    StartCoroutine(ShowPowerError());

                    // Also play the sound on the reader
                    currentReader.TryAccess(inventory);
                }
                else
                {
                    // Power is ON, proceed normally
                    currentReader.TryAccess(inventory);
                }
                return;
            }

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

            // Check for Keycard Reader
            var reader = hit.collider.GetComponent<KeycardReader>();
            if (reader != null)
            {
                // Pass the inventory so the reader can check for the card
                reader.TryAccess(inventory);
                return;
            }

            // Check for Breakable Glass
            var glass = hit.collider.GetComponent<BreakableGlass>();
            if (glass != null)
            {
                glass.Smash();
                return;
            }

            // Check for Destructible Obstacle (The Blockage)
            var obstacle = hit.collider.GetComponent<DestructibleObstacle>();
            if (obstacle != null)
            {
                obstacle.TryDestroy(inventory);
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
        // Coroutine to flash the error message
        IEnumerator ShowPowerError()
        {
            // 1. Hide the normal UI
            ReaderLockedUI.SetActive(false);
            ReaderUnlockUI.SetActive(false);

            // 2. Show the Error
            ReaderNoPowerUI.SetActive(true);

            // 3. Wait for 3 seconds
            yield return new WaitForSeconds(3f);

            // 4. Hide Error (The Update loop will bring back the normal UI automatically)
            ReaderNoPowerUI.SetActive(false);
        }
    }
}
