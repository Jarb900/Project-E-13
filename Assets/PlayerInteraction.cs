using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction")]
    public float interactionDistance = 3f; // How far the player can interact
    public KeyCode interactionKey = KeyCode.E; // The key to interact
    public Transform playerCamera; // Drag your Main Camera here

    // 1. This is your "inventory"
    public bool hasKey = false;

    void Update()
    {
        // 2. This handles the "interaction"
        if (Input.GetKeyDown(interactionKey))
        {
            // Shoot a ray from the camera
            Ray ray = new Ray(playerCamera.position, playerCamera.forward);
            RaycastHit hit;

            // If the ray hits something within interactionDistance
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                // Check if the object we hit has a "Lock" script on it
                Lock lockScript = hit.collider.GetComponent<Lock>();
                if (lockScript != null)
                {
                    // If it's a lock, try to unlock it
                    lockScript.TryUnlock(this);
                }

                // Check if the object we hit is a "Key"
                Key keyScript = hit.collider.GetComponent<Key>();
                if (keyScript != null)

                {
                    // If it's a key, pick it up
                    keyScript.Pickup(this);
                }
            }

        }
    }
}
