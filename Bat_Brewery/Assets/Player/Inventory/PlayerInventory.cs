using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// The players inventory, currently only holds ingredients.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();

    void Start()
    {
        GameEventsManager.instance.inventoryEvents.onItemAdded += AddItem;
    }

    void OnDisable()
    {
        GameEventsManager.instance.inventoryEvents.onItemAdded -= AddItem;
    }

    /// <summary>
    /// Adds item to inventory, if it already exists count is increased,
    /// else a new item is created and added.
    /// </summary>
    /// <param name="ing">Item to be added.</param>
    private void AddItem(ItemSO ing) {
        InventoryItem temp = items.Find(ingredient => ingredient.item == ing);
        if(temp != null) {
            temp.count ++;
        } else {
            items.Add(new InventoryItem(ing));
        }
    }

    /// <summary>
    /// Tries to remove an item from inventory. If ingredient exists
    /// its count is lowered in inventory, or removed if it was the last
    /// and returns true.
    /// If item doesn't exist the method returns false and nothing
    /// happens.
    /// </summary>
    /// <param name="ing">Ingredient to be removed.</param>
    /// <returns></returns>
    public bool RemoveItem(ItemSO ing) {
        InventoryItem temp = items.Find(item => item.item == ing);
        if(temp != null) {
            temp.count --;
            if(temp.count == 0) {
            items.Remove(temp);
            }
            GameEventsManager.instance.inventoryEvents.ItemWasRemoved(ing);
            return true;
        } 
        InsufficientIngredientsPopup();
        return false;
    }

    /// <summary>
    /// Tries to remove ingredients from inventory. If ingredient exists
    /// its count is reduced by the amount, and removed if it was the last
    /// and returns true.
    /// If ingredient doesn't exist the method returns false and nothing
    /// happens.
    /// If the ingredient existed but the amount in the inventory was not enough as the amount specified to be removed, 
    /// none of the ingredient is removed and the method returns false.
    /// </summary>
    /// <param name="ing">Ingredient to be removed.</param>
    /// <returns></returns>
    public bool RemoveItem(ItemSO ing, int amount)
    {
        InventoryItem temp = items.Find(ingredient => ingredient.item == ing);
        if(temp != null)
        {
            if(temp.count < amount)
            {
                InsufficientIngredientsPopup();
                return false;
            }
            temp.count -= amount;
            if(temp.count == 0)
            {
                items.Remove(temp);
            }
            GameEventsManager.instance.inventoryEvents.ItemWasRemoved(ing);
            return true;
        }
        InsufficientIngredientsPopup();
        return false;
    }

    /// <summary>
    /// Tries to remove ingredients from inventory. If all ingredients exist with 
    /// the specified amount then those are reduced in the inventory and it returns true.
    /// If ingredient doesn't exist the method returns false.
    /// If not all ingredients existed with the specified amount the method returns false and no
    /// ingredients are removed.
    /// </summary>
    /// <param name="ingredientsToRemove"></param>
    /// <returns></returns>
    public bool RemoveItem(InventoryItem[] ingredientsToRemove)
    {
        if (!EnoughIngredients(ingredientsToRemove)) return false;
        foreach(InventoryItem invIng in ingredientsToRemove)
        {
            RemoveItem(invIng.item, invIng.count);
        }

        return true;
    }

    /// <summary>
    /// Returns 0 if ingredient doesn't exist in inventory.
    /// Else returns ingredient count.
    /// </summary>
    /// <param name="ing">Ingredient to be checked.</param>
    /// <returns>Number of ingredient.</returns>
    public int CheckIngredientCount(ItemSO ing){
        InventoryItem temp = items.Find(ingredient => ingredient.item == ing);
        if(temp != null) {
            return temp.count;
        } 
        return 0;
    }

    /// <summary>
    /// Returns true if the inventory has equal to or more than the specified required ingredients
    /// </summary>
    /// <param name="required"></param>
    /// <returns></returns>
    public bool EnoughIngredients(InventoryItem[] required)
    {
        foreach(InventoryItem invIng in required)
        {
            if(CheckIngredientCount(invIng.item) < invIng.count) return false;
        }

        return true;
    }

    public void InsufficientIngredientsPopup()
    {
        //TODO: ADD A UI EFFECT THAT POPS UP ON THE SCREEN THAT TELLS THE PLAYER THAT THEY HAVE INSUFFICIENT INGREDIENTS
        Debug.Log("The player has insufficient ingredients for the operation");
    }
}

/// <summary>
/// Wrapper for handling ingredients in inventory.
/// </summary>
[Serializable]
public class InventoryItem {
    public ItemSO item;
    public int count;

    public InventoryItem(ItemSO ing) {
        item = ing;
        count = 1;
    }
}
