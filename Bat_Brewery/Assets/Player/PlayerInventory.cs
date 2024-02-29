using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The players inventory, currently only holds ingredients.
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public List<InventoryIngredient> ingredients;

    public void AddIngredient(WorldIngredient ing) {
        InventoryIngredient newIng = new InventoryIngredient(ing);
        ingredients.Add(newIng);
    }

    public void RemoveIngredient(InventoryIngredient item) {
        ingredients.Remove(item);
    }
}
