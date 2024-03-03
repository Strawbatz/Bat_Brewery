using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is a quest step that requires that the player gathers specific ingredients.
/// The quest defines how much of each ingredient that is needed.
/// </summary>
public class GatherIngredientsQS : QuestStep
{
    [SerializeField] List<InventoryIngredient> ingredientsRequired;

    [Header("Destroy ingredients when completed")]
    [SerializeField] bool destroyIngredients;

    private PlayerInventory playerInventory;

    private void Start() 
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        GameEventsManager.instance.inventoryEvents.onPickUpIngredient += IngredientPickedUp;

        CheckIfCompleted();
    }

    private void OnDisable() 
    {
        GameEventsManager.instance.inventoryEvents.onPickUpIngredient -= IngredientPickedUp;
    }

    private void IngredientPickedUp(Ingredient ingredient)
    {
        if(!ingredientsRequired.Exists(invIng => invIng.ingredient == ingredient)) return;
        CheckIfCompleted();
    }

    private void CheckIfCompleted()
    {
        foreach (InventoryIngredient inventoryIngredient in ingredientsRequired)
        {
            if(playerInventory.CheckIngredientCount(inventoryIngredient.ingredient) < inventoryIngredient.count) return;
        }

        if(destroyIngredients)
        {
            foreach(InventoryIngredient inventoryIngredient in ingredientsRequired)
            {
                playerInventory.RemoveIngredient(inventoryIngredient.ingredient, inventoryIngredient.count);
            }
        }

        FinishQuestStep();
    }
}