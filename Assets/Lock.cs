using UnityEngine;

public class Lock : MonoBehaviour
{
    // Drag your Door (the Cube) into this slot in the Inspector
    public GameObject doorToOpen;

    // We'll get the Animator from the door
    private Animator doorAnimator;

    void Start()
    {
        // Get the Animator component from the door object
        if (doorToOpen != null)
        {
            doorAnimator = doorToOpen.GetComponent<Animator>();
        }
    }

    // This function will be called by the PlayerInteraction script
    public void TryUnlock(PlayerInteraction player)
    {
        // 1. Check if the player has the key
        if (player.hasKey)
        {
            Debug.Log("Key fits! Opening door.");

            // 2. Open the door (by deactivating it)
            if (doorToOpen != null)
            {
                doorAnimator.SetTrigger("Open");
            }

        }
        else
        {
            // 4. (Optional) Tell the player they need a key
            Debug.Log("This door is locked. I need a key.");
        }
    }
}