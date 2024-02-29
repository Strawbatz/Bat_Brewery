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

    /// <summary>
    /// Adds ingredient to inventory, if it already exists count is increased,
    /// else a new ingredient is created and added.
    /// </summary>
    /// <param name="ing">Ingredient to be added.</param>
    public void AddIngredient(Ingredient ing) {
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
            return true;
        } 
        return false;
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
