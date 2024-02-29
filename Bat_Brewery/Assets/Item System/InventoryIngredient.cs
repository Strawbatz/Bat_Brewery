using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for ingredients in inventories. It picks its 
/// inventory img at random from the 2 inventory img
/// that exists. 
/// </summary>
public class InventoryIngredient : ScriptableObject
{
    public TaggableItem itemTag;
    public Sprite inventoryImg;
    public IngredientType ingredientType;

    public void Create(WorldIngredient item) {
        itemTag = item.itemTag;
        ingredientType = item.ingredientType;
        if(Random.Range(0,1) == 0) {
            inventoryImg = itemTag.visualTag.GetInventoryImg();
        } else {
            inventoryImg = itemTag.visualTag.GetInventoryImg2();
        }
    }
}
