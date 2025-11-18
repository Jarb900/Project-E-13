using NavKeypad;
using UnityEngine;

namespace Main.Scripts
{
    public class KeypadActivator : MonoBehaviour
    {
        // Drag the SafeInteract script into this slot in the inspector
        public SafeInteract safeInteractReference; 

        void Start()
        {
            // 1. Hook up the Close button
            // (You must manually set up the Close button in the Inspector,
            // pointing to this KeypadActivator script's CloseSelf function)

            // 2. Hook up the Enter button
            Keypad keypadScript = GetComponent<Keypad>();
            if (keypadScript != null && safeInteractReference != null)
            {
                // Hook the Keypad's success event to the Safe's Unlock function
                keypadScript.OnAccessGranted.AddListener(safeInteractReference.UnlockSafe);
            }
        }
    }
}