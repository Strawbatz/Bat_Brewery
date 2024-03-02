using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatherIngredientsQS : QuestStep
{
    [SerializeField] List<InventoryIngredient> ingredientsRequired;

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

        FinishQuestStep();
    }
}