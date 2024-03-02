using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC component for NPCs that are part of quests
/// </summary>
public class QuestNPC : TalkableNPC
{
    protected Queue<TextAsset> dialogueQueue = new Queue<TextAsset>();
    public override void Talk(TextAsset dialogue)
    {
        DialogueManager.GetInstance().EnterDialogueMode(dialogue, this);
    }

    protected override void Interact()
    {
        if(dialogueQueue.Count > 0)
        {
            Talk(dialogueQueue.Dequeue());
        } else if(defaultDialogue)
        {
            Talk(defaultDialogue);
        }
    }

    /// <summary>
    /// Queues the dialogue for a quest to this NPC
    /// </summary>
    /// <param name="questDialogue"></param>
    public void AddQuestDialogue(TextAsset questDialogue)
    {
        dialogueQueue.Enqueue(questDialogue);
    }
}
