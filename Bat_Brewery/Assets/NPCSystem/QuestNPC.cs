using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC component for NPCs that are part of quests
/// </summary>
public class QuestNPC : TalkableNPC
{
    protected Queue<DialogueQS> dialogueQueue = new Queue<DialogueQS>();
    public override void Talk(TextAsset dialogue)
    {
        DialogueManager.GetInstance().EnterDialogueMode(dialogue, this);
    }

    protected override void Interact()
    {
        if(dialogueQueue.Count > 0)
        {
            Talk(dialogueQueue.Dequeue().GetQuestStory());
        } else if(defaultDialogue)
        {
            Talk(defaultDialogue);
        }
    }

    /// <summary>
    /// Queues the dialogue for a quest to this NPC
    /// </summary>
    /// <param name="questDialogue"></param>
    public void AddQuestDialogue(DialogueQS questDialogue)
    {
        dialogueQueue.Enqueue(questDialogue);
    }
}
