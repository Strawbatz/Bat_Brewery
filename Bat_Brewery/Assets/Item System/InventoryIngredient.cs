using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for ingredients in inventories. It picks its 
/// inventory img at random from the 2 inventory img
/// that exists. 
/// </summary>
[Serializable] 
public class InventoryIngredient
{
    public TaggableItem itemTag;
    public Sprite inventoryImg;
    public IngredientType ingredientType;

    public InventoryIngredient(WorldIngredient item) {
        itemTag = item.itemTag;
        ingredientType = item.ingredientType;
        inventoryImg = itemTag.visualTag.GetRandomInventoryImg();
    }
}
