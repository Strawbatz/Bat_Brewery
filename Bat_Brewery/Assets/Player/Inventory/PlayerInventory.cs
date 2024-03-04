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
    public List<InventoryIngredient> ingredients = new List<InventoryIngredient>();

    void Start()
    {
        GameEventsManager.instance.inventoryEvents.onPickUpIngredient += AddIngredient;
    }

    void OnDisable()
    {
        GameEventsManager.instance.inventoryEvents.onPickUpIngredient -= AddIngredient;
    }

    /// <summary>
    /// Adds ingredient to inventory, if it already exists count is increased,
    /// else a new ingredient is created and added.
    /// </summary>
    /// <param name="ing">Ingredient to be added.</param>
    private void AddIngredient(Ingredient ing) {
        InventoryIngredient temp = ingredients.Find(ingredient => ingredient.ingredient == ing);
        if(temp != null) {
            temp.count ++;
        } else {
            ingredients.Add(new InventoryIngredient(ing));
        }
    }

    /// <summary>
    /// Tries to remove an ingredient from inventory. If ingredient exists
    /// its count is lowered in inventory, or removed if it was the last
    /// and returns true.
    /// If ingredient doesn't exist the method returns false and nothing
    /// happens.
    /// </summary>
    /// <param name="ing">Ingredient to be removed.</param>
    /// <returns></returns>
    public bool RemoveIngredient(Ingredient ing) {
        InventoryIngredient temp = ingredients.Find(ingredient => ingredient.ingredient == ing);
        if(temp != null) {
            temp.count --;
            if(temp.count == 0) {
            ingredients.Remove(temp);
            }
            GameEventsManager.instance.inventoryEvents.IngredientWasRemoved(ing);
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
    public bool RemoveIngredient(Ingredient ing, int amount)
    {
        InventoryIngredient temp = ingredients.Find(ingredient => ingredient.ingredient == ing);
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
                ingredients.Remove(temp);
            }
            GameEventsManager.instance.inventoryEvents.IngredientWasRemoved(ing);
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
    public bool RemoveIngredient(InventoryIngredient[] ingredientsToRemove)
    {
        if (!EnoughIngredients(ingredientsToRemove)) return false;
        foreach(InventoryIngredient invIng in ingredientsToRemove)
        {
            RemoveIngredient(invIng.ingredient, invIng.count);
        }

        return true;
    }

    /// <summary>
    /// Returns 0 if ingredient doesn't exist in inventory.
    /// Else returns ingredient count.
    /// </summary>
    /// <param name="ing">Ingredient to be checked.</param>
    /// <returns>Number of ingredient.</returns>
    public int CheckIngredientCount(Ingredient ing){
        InventoryIngredient temp = ingredients.Find(ingredient => ingredient.ingredient == ing);
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
    public bool EnoughIngredients(InventoryIngredient[] required)
    {
        foreach(InventoryIngredient invIng in required)
        {
            if(CheckIngredientCount(invIng.ingredient) < invIng.count) return false;
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
public class InventoryIngredient {
    public Ingredient ingredient;
    public int count;

    public InventoryIngredient(Ingredient ing) {
        ingredient = ing;
        count = 1;
    }
}
