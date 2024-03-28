using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// The players inventory, currently only holds ingredients and items;
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public List<InventoryItem> ingredients = new List<InventoryItem>();
    [SerializeField] int maxIngredientSlots = 4;

    void Start()
    {
        GameEventsManager.instance.inventoryEvents.onItemAdded += AddItem;
    }

    void OnDisable()
    {
        GameEventsManager.instance.inventoryEvents.onItemAdded -= AddItem;
    }

    private List<InventoryItem> FindList(ItemSO item) {
        if(item.GetType().IsSubclassOf(typeof(IngredientSO)) || item.GetType().Equals(typeof(IngredientSO))) {
            return ingredients;
        } else if(item.GetType().IsSubclassOf(typeof(QuestItemSO)) || item.GetType().Equals(typeof(QuestItemSO))){
            return items;
        }
        Debug.LogError("Trying to add illegal item to inventory. Item type: " + item.GetType() + ", Item name: " + item.name);
        return null;
    } 

    /// <summary>
    /// Adds item to inventory, if it already exists count is increased,
    /// else a new item is created and added.
    /// </summary>
    /// <param name="item">Item to be added.</param>
    private void AddItem(ItemSO item) {
        List<InventoryItem> activeList = FindList(item);
        if(activeList == null) return;
        InventoryItem temp = activeList.Find(obj => obj.item == item);
        if(temp != null) {
            temp.count ++;
        } else {
            activeList.Add(new InventoryItem(item));
        }

        if (ingredients.Count > maxIngredientSlots) ingredients.RemoveAt(ingredients.Count-1);
    }

    /// <summary>
    /// Tries to remove an item from inventory. If item exists
    /// its count is lowered in inventory, or removed if it was the last
    /// and returns true.
    /// If item doesn't exist the method returns false and nothing
    /// happens.
    /// </summary>
    /// <param name="item">item to be removed.</param>
    /// <returns></returns>
    public bool RemoveItem(ItemSO item) {
        List<InventoryItem> activeList = FindList(item);
        if(activeList == null) return false;
        InventoryItem temp = activeList.Find(obj => obj.item == item);
        if(temp != null) {
            temp.count --;
            if(temp.count == 0) {
            activeList.Remove(temp);
            }
            GameEventsManager.instance.inventoryEvents.ItemWasRemoved(item);
            return true;
        } 
        InsufficientItemPopup();
        return false;
    }

    /// <summary>
    /// Tries to remove items from inventory. If item exists
    /// its count is reduced by the amount, and removed if it was the last
    /// and returns true.
    /// If item doesn't exist the method returns false and nothing
    /// happens.
    /// If the item existed but the amount in the inventory was not enough as the amount specified to be removed, 
    /// none of the item is removed and the method returns false.
    /// </summary>
    /// <param name="item">item to be removed.</param>
    /// <returns></returns>
    public bool RemoveItem(ItemSO item, int amount) {
        List<InventoryItem> activeList = FindList(item);
        if(activeList == null) return false;
        InventoryItem temp = activeList.Find(obj => obj.item == item);
        if(temp != null)
        {
            if(temp.count < amount)
            {
                InsufficientItemPopup();
                return false;
            }
            temp.count -= amount;
            if(temp.count == 0)
            {
                activeList.Remove(temp);
            }
            GameEventsManager.instance.inventoryEvents.ItemWasRemoved(item);
            return true;
        }
        InsufficientItemPopup();
        return false;
    }

    /// <summary>
    /// Tries to remove items from inventory. If all items exist with 
    /// the specified amount then those are reduced in the inventory and it returns true.
    /// If item doesn't exist the method returns false.
    /// If not all item existed with the specified amount the method returns false and no
    /// items are removed.
    /// </summary>
    /// <param name="itemToRemove"></param>
    /// <returns></returns>
    public bool RemoveItem(InventoryItem[] itemToRemove)
    {
        if (!EnoughItems(itemToRemove)) return false;
        foreach(InventoryItem invItem in itemToRemove)
        {
            RemoveItem(invItem.item, invItem.count);
        }

        return true;
    }

    /// <summary>
    /// Returns 0 if item doesn't exist in inventory.
    /// Else returns item count.
    /// </summary>
    /// <param name="item">item to be checked.</param>
    /// <returns>Number of item.</returns>
    public int CheckItemCount(ItemSO item){
        List<InventoryItem> activeList = FindList(item);
        if(activeList == null) return 0;
        InventoryItem temp = activeList.Find(obj => obj.item == item);
        if(temp != null) {
            return temp.count;
        } 
        return 0;
    }

    /// <summary>
    /// Returns true if the inventory has equal to or more than the specified required items
    /// </summary>
    /// <param name="required"></param>
    /// <returns></returns>
    public bool EnoughItems(InventoryItem[] required)
    {
        foreach(InventoryItem invItem in required)
        {
            if(CheckItemCount(invItem.item) < invItem.count) return false;
        }

        return true;
    }

    public void InsufficientItemPopup()
    {
        //TODO: ADD A UI EFFECT THAT POPS UP ON THE SCREEN THAT TELLS THE PLAYER THAT THEY HAVE INSUFFICIENT INGREDIENTS
        Debug.Log("The player has insufficient ingredients or items for the operation");
    }

    /// <summary>
    /// Adds more ingredient slots
    /// </summary>
    /// <param name="slots"></param>
    public void AddIngredientSlots(int slots)
    {
        maxIngredientSlots += slots;
    }

    /// <summary>
    /// Returns the max number of ingredient slots
    /// </summary>
    /// <returns></returns>
    public int GetMaxIngredientSlots()
    {
        return maxIngredientSlots;
    }

    /// <summary>
    /// Returns true if the inventory has an available ingredient slot
    /// </summary>
    /// <returns></returns>
    public bool HasAvailableIngredientSlot()
    {
        return ingredients.Count < maxIngredientSlots;
    }
}

/// <summary>
/// Wrapper for handling items in inventory.
/// </summary>
[Serializable]
public class InventoryItem {
    public ItemSO item;
    public int count;

    public InventoryItem(ItemSO obj) {
        item = obj;
        count = 1;
    }
}
