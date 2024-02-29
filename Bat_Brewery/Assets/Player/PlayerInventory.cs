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

    public void AddIngredient(Ingredient ing) {
        InventoryIngredient temp = ingredients.Find(ingredient => ingredient.ingredient == ing);
        if(temp != null) {
            temp.count ++;
        } else {
            ingredients.Add(new InventoryIngredient(ing));
        }
    }

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

    public int CheckIngredientCount(Ingredient ing){
        InventoryIngredient temp = ingredients.Find(ingredient => ingredient.ingredient == ing);
        if(temp != null) {
            return temp.count;
        } 
        return 0;
    }
}

[Serializable]
public class InventoryIngredient {
    public Ingredient ingredient;
    public int count;

    public InventoryIngredient(Ingredient ing) {
        ingredient = ing;
        count = 1;
    }
}
