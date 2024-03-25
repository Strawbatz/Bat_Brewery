using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

/// <summary>
/// This quest step requires the player to gift a specified amount of ingredients to an NPC in a dialogue.
/// </summary>
public class GiftItemsQS : DialogueQS
{
    [Header("The gift story needs to be a choice")]
    [SerializeField] TextAsset giftStory;
    [SerializeField] int giftChoiceIndex;

    [Header("Is played if the player has insufficient resources")]
    [SerializeField] TextAsset notEnoughStory;
    [SerializeField] string npcName;
    [SerializeField] InventoryItem[] ingredientsRequired;
    QuestNPC npc;
    PlayerInventory playerInventory;
    TextAsset story;

    private void Start()
    {
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        GameObject obj = GameObject.Find(npcName);
        if(!obj) Debug.LogError("The NPC with name " + npcName + " could not be found");

        npc = obj.GetComponent<QuestNPC>();
        npc.AddQuestDialogue(this);

        foreach(InventoryItem item in ingredientsRequired)
        {
            if(item.item.GetType().Equals(typeof(IngredientSO)))
                GameEventsManager.instance.questEvents.TextbookDescHeard((IngredientSO)item.item);
        }

        GameEventsManager.instance.dialogueEvents.onChoiceMade += ChoiceMade;
        GameEventsManager.instance.dialogueEvents.onDialogueEnded += DialogueEnded;
    }
    private void OnDisable() 
    {
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= ChoiceMade;
        GameEventsManager.instance.dialogueEvents.onDialogueEnded -= DialogueEnded;
    }

    public override TextAsset GetQuestStory()
    {
        if(notEnoughStory == null || playerInventory.EnoughIngredients(ingredientsRequired))
        {
            story = giftStory;
            return story;
        } 

        story = notEnoughStory;
        return story;
    }

    private void DialogueEnded(string storyId)
    {
        if(notEnoughStory != null && storyId.Equals(notEnoughStory.name) && story == notEnoughStory)
            npc.AddQuestDialogue(this);
    }

    private void ChoiceMade(string storyId, int choiceIndex)
    {
        if(storyId.Equals(giftStory.name))
        {
            if(choiceIndex == giftChoiceIndex)
                CheckIfCompleted();
            else
                npc.AddQuestDialogue(this);
        }
    }

    private void CheckIfCompleted()
    {
        if(!playerInventory.RemoveItem(ingredientsRequired))
        {
            playerInventory.InsufficientIngredientsPopup();
            npc.AddQuestDialogue(this);
            return;
        }
        
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= ChoiceMade;
        GameEventsManager.instance.dialogueEvents.onDialogueEnded -= DialogueEnded;
        FinishQuestStep();
    }
}
