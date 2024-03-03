using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

public class GiftIngredientsQS : QuestStep
{
    [Header("The story needs to be a choice of wether to gift the ingredients")]
    [SerializeField] TextAsset story;
    [SerializeField] int collectChoiceIndex;
    [SerializeField] string npcName;
    [SerializeField] InventoryIngredient[] ingredientsRequired;
    QuestNPC npc;
    PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        GameObject obj = GameObject.Find(npcName);
        if(!obj) Debug.LogError("The NPC with name " + npcName + " could not be found");

        npc = obj.GetComponent<QuestNPC>();
        npc.AddQuestDialogue(story);
        GameEventsManager.instance.dialogueEvents.onChoiceMade += ChoiceMade;
    }
    private void OnDisable() 
    {
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= ChoiceMade;
    }
    private void ChoiceMade(string storyId, int choiceIndex)
    {
        if(storyId.Equals(story.name))
        {
            if(choiceIndex == collectChoiceIndex)
                CheckIfCompleted();
            else
                npc.AddQuestDialogue(story);
        }
    }

    private void CheckIfCompleted()
    {
        foreach (InventoryIngredient inventoryIngredient in ingredientsRequired)
        {
            if(playerInventory.CheckIngredientCount(inventoryIngredient.ingredient) < inventoryIngredient.count)
            {
                playerInventory.InsufficientIngredientsPopup();
                npc.AddQuestDialogue(story);
                return;
            }
        }

        foreach(InventoryIngredient inventoryIngredient in ingredientsRequired)
        {
            playerInventory.RemoveIngredient(inventoryIngredient.ingredient, inventoryIngredient.count);
        }
        
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= ChoiceMade;
        FinishQuestStep();
    }
}
