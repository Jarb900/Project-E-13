using ElmanGameDevTools.FirstPersonControllerPro.Scripts.PlayerSystem;
using NavKeypad;
using UnityEngine;

namespace Main.Scripts
{
    public class SafeInteract : MonoBehaviour
    {
        [Header("Keypad Settings")] public GameObject keypadPrefab; // Drag your Keypad Prefab here
        public Transform spawnPoint; // Where the keypad appears (optional, or use screen space)

        [Header("Safe Settings")] public Animator objectAnimator; // The Hinge Animator
        public string openTrigger = "Open";

        private GameObject currentKeypad;
        private PlayerController playerController;

        private bool isKeypadActive = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // Called by PlayerInteraction
        public void ShowKeypad(PlayerController controller)
        {
            if (isKeypadActive) return;

            // 1. Disable Player Movement & Camera
            playerController = controller;
            playerController.enabled = false;

            // 2. Show Mouse Cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 3. Spawn Keypad - SIMPLIFIED
            // We let Unity handle the screen placement since it's an Overlay Canvas
            currentKeypad = Instantiate(keypadPrefab);
        
            // 4. Setup Keypad
            Keypad keypadScript = currentKeypad.GetComponent<Keypad>();
            keypadScript.Initialize(this); 

            // Listen for the "Success" event
            keypadScript.OnAccessGranted.AddListener(UnlockSafe);

            isKeypadActive = true;
        }

        // Update handles the mouse clicking ONLY when keypad is open
        void Update()
        {
            if (!isKeypadActive) return;

            // This replaces KeypadInteractionFPV
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton button))
                    {
                        button.PressButton();
                    }
                }
            }

            // Optional: Press Escape to close
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseKeypad();
            }
        }

        public void UnlockSafe()
        {
            Debug.Log("Access Granted!");
            if (objectAnimator != null)
            {
                objectAnimator.SetTrigger(openTrigger);
            }

            // Disable the collider so we can't click the safe door again
            GetComponent<Collider>().enabled = false;

            CloseKeypad();
        }

        public void CloseKeypad()
        {
            // 1. Destroy Keypad UI
            if (currentKeypad != null) Destroy(currentKeypad);

            // 2. Re-enable Player
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            // 3. Lock Cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            isKeypadActive = false;
        }
    }
}
