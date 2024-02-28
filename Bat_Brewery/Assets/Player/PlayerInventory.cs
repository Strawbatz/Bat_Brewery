using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<InventoryIngredient> ingredients;

    public void AddIngredient(WorldIngredient ing) {
        InventoryIngredient newIng = ScriptableObject.CreateInstance<InventoryIngredient>();
        newIng.Create(ing.itemType);
        ingredients.Add(newIng);
    }

    public void RemoveIngredient(InventoryIngredient item) {
        ingredients.Remove(item);
    }
}
