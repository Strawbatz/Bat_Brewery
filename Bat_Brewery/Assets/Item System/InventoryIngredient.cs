using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryIngredient : ScriptableObject
{
    
    public TaggableItem itemType;
    public Sprite inventoryImg;

    public void Create(TaggableItem taggableItem) {
        itemType = taggableItem;
        if(Random.Range(0,1) == 0) {
            inventoryImg = taggableItem.visualTag.GetInventoryImg();
        } else {
            inventoryImg = taggableItem.visualTag.GetInventoryImg2();
        }
    }
}
