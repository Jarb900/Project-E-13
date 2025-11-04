using UnityEngine;

public class Key : MonoBehaviour
{
    // This function will be called by the PlayerInteraction script
    public void Pickup(PlayerInteraction player)
    {
        // 1. Tell the player script that it now has the key
        player.hasKey = true;

        // 2. Destroy the key object from the world
        Destroy(gameObject);
    }
}