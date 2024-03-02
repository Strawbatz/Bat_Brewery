using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A prerequisite to a quest that a specific dialogue choice has been made
/// </summary>
public class QuestPrerequisiteChoice : QuestPrerequisite
{
    [SerializeField] TextAsset story;
    [SerializeField] int orderInConversation = 0;
    [SerializeField] int choiceNeeded;

    private bool prerequisiteMet = false;
    private int order = 0;
    void Start()
    {
        GameEventsManager.instance.dialogueEvents.onChoiceMade += CheckChoice;
        GameEventsManager.instance.dialogueEvents.onDialogueStarted += ResetOrder;
    }
    void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= CheckChoice;
        GameEventsManager.instance.dialogueEvents.onDialogueStarted -= ResetOrder;
    }

    private void ResetOrder(string storyId)
    {
        if(storyId.Equals(story.name)) order = 0;
    }

    private void CheckChoice(string storyId, int choiceMade)
    {
        if(storyId.Equals(story.name))
        {
            if(order == orderInConversation)
            {
                if(choiceMade == choiceNeeded) 
                {
                    prerequisiteMet = true;
                    GameEventsManager.instance.dialogueEvents.onDialogueStarted -= ResetOrder;
                    GameEventsManager.instance.dialogueEvents.onChoiceMade -= CheckChoice;
                }
            }
            order++;
        }
    }

    public override bool PrerequisiteIsMet()
    {
        return prerequisiteMet;
    }
}
