using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

/// <summary>
/// This is a quest step that requires that the player gathers specific ingredients.
/// The quest defines how much of each ingredient that is needed.
/// </summary>
public class GatherItemsQS : QuestStep
{
    [SerializeField] List<InventoryItem> ingredientsRequired;

    [Header("Destroy ingredients when completed")]
    [SerializeField] bool destroyIngredients;

    private PlayerInventory playerInventory;

    private void Start() 
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        GameEventsManager.instance.inventoryEvents.onItemAdded += ItemPickedUp;
        
        foreach(InventoryItem item in ingredientsRequired)
        {
            if(item.item.GetType().Equals(typeof(IngredientSO)))
                GameEventsManager.instance.questEvents.TextbookDescHeard((IngredientSO)item.item);
        }

        CheckIfCompleted();
    }

    private void OnDisable() 
    {
        GameEventsManager.instance.inventoryEvents.onItemAdded -= ItemPickedUp;
    }

    private void ItemPickedUp(ItemSO item)
    {
        if(!ingredientsRequired.Exists(invIng => invIng.item.GetId().Equals(item.GetId()))) return;
        CheckIfCompleted();
    }

    private void CheckIfCompleted()
    {
        if(!playerInventory.EnoughIngredients(ingredientsRequired.ToArray())) return;

        if(destroyIngredients)
        {
            if(!playerInventory.RemoveItem(ingredientsRequired.ToArray())) return;
        }

        FinishQuestStep();
    }
}