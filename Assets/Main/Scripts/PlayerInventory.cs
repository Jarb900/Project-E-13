using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Main.Scripts
{
    [System.Serializable]
    public class PreAttachedEquipment
    {
        public ItemData item;
        public GameObject equipmentObject;
    }
    public class PlayerInventory : MonoBehaviour
    {
        private static readonly int IsEquipped = Animator.StringToHash("IsEquipped");

        [Header("Equip Settings")]
        [Tooltip("The parent transform for instantiating new items.")]
        public Transform equipSlot; // Drag your 'RightHand' bone here
        [Header("Animation")]
        public Animator anim;
        
        
        [Tooltip("A list of items that are already attached to the player's skeleton.")]
        public List<PreAttachedEquipment> preAttachedItems = new List<PreAttachedEquipment>();
        
        // The "Backpack"
        public List<ItemData> items = new List<ItemData>();
    
        // The item currently in our hand
        private GameObject currentEquippedItemObject = null;
        private ItemData currentEquippedItemData = null;

        private void Start()
        {
            // Deactivate all pre-attached items on game start
            foreach (var item in preAttachedItems.Where
                         (item => item.equipmentObject != null))
            {
                item.equipmentObject.SetActive(false);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Adds an item to our inventory list.
        /// </summary>
        public void AddItem(ItemData item)
        {
            Debug.Log("Picked up " + item.itemName);
            items.Add(item);
        }

        /// <summary>
        /// Checks if we have a specific item in our list.
        /// </summary>
        public bool HasItem(ItemData item)
        {
            return items.Contains(item);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Puts an item into the player's hand.
        /// </summary>
        public void EquipItem(ItemData itemToEquip)
        {
            // 1. Check if we even have this item
            if (!HasItem(itemToEquip))
            {
                Debug.LogError("Equip Failed: Player doesn't have " + itemToEquip.itemName + " in inventory.");
                return;
            }
            
            // 2. Check if we're already holding it
            if (itemToEquip == currentEquippedItemData)
            {
                Debug.LogWarning("Equip Canceled: Already holding this item.");
                return; // We're already holding this item
            }

            // 3. Deactivate/Destroy whatever we are already holding
            if (currentEquippedItemObject != null)
            {
                Debug.Log("Unequipping " + currentEquippedItemData.itemName);
                anim.SetBool(IsEquipped, false);
                // Check if the old item was pre-attached or instantiated
                var wasPreAttached = false;
                foreach (var item in preAttachedItems.Where
                             (item => item.item == currentEquippedItemData))
                {
                    item.equipmentObject.SetActive(false); // Just deactivate it
                    wasPreAttached = true;
                    break;
                }

                if (!wasPreAttached)
                {
                    Destroy(currentEquippedItemObject); // Destroy the instantiated object
                }
            }

            // 4. Equip the new item
            Debug.Log("Equipping: " + itemToEquip.itemName);
            currentEquippedItemData = itemToEquip; // Store what we're holding
            var foundPreAttached = false;

            // First, try to find a pre-attached version
            foreach (var item in preAttachedItems.Where
                         (item => item.item == itemToEquip))
            {
                currentEquippedItemObject = item.equipmentObject;
                if (currentEquippedItemObject != null)
                {
                    currentEquippedItemObject.SetActive(true); 
                }
                foundPreAttached = true;
                break;
            }

            // If no pre-attached version, instantiate it from the prefab
            if (!foundPreAttached)
            {
                Debug.LogWarning("Could not find pre-attached item. Did the ItemData references match?");
            }
            
            if (foundPreAttached || itemToEquip.equipPrefab == null) return;
            currentEquippedItemObject = Instantiate(itemToEquip.equipPrefab, equipSlot);
            currentEquippedItemObject.transform.localPosition = Vector3.zero;
            currentEquippedItemObject.transform.localRotation = Quaternion.identity;

            if (anim != null)
            {
                if (currentEquippedItemData.itemType == ItemType.Weapon)
                {
                    anim.SetBool(IsEquipped, true);
                }
                else
                {
                    anim.SetBool(IsEquipped, false);
                }
            }
        }
    }
}