using UnityEngine;

namespace Main.Scripts
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Item")]
        [Tooltip("The ItemData file that this pickup represents.")]
        public ItemData itemData; // Drag your ItemData asset here

        // This will be called by our PlayerInteraction script
        public void Pickup(PlayerInventory inventory)
        {
            // Add the item to the inventory
            inventory.AddItem(itemData);

            // If the item is a Weapon, also equip it instantly
            if (itemData.itemType == ItemType.Weapon)
            {
                inventory.EquipItem(itemData);
            }

            // Destroy the pickup from the world
            Destroy(gameObject);
        }
    }
}
