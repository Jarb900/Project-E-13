using UnityEngine;

namespace Main.Scripts
{
    [CreateAssetMenu(fileName = "New ItemData", menuName = "Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Info")]
        public string itemName = "New Item";
        public Sprite itemIcon; // For a future UI

        [Header("Equipping")]
        public GameObject equipPrefab; // The 3D model to put in the player's hand

        [Header("Item Type")]
        public ItemType itemType;
    }

// This defines what *kind* of item it is
    public enum ItemType
    {
        Key,
        Weapon,
        Generic
    }
}