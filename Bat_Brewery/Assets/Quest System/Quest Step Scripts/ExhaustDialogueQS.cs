using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A quest step that the requires the player to talk to a specific npc. 
/// This quest step is finished when the player has exhausted the specified dialogue with the npc.
/// The quest step automatically sends the request to the npc with the specified dialogue. 
/// </summary>
public class ExhaustDialogueQS : DialogueQS
{
    [SerializeField] TextAsset story;

    //Note that this is the name of the object of the npc.
    [Tooltip("Note that this is the name of the object of the NPC")]
    [SerializeField] string npcName;

    QuestNPC npc;
    private void Start()
    {
        GameObject obj = GameObject.Find(npcName);
        if(!obj) Debug.LogError("The NPC with name " + npcName + " could not be found");

        npc = obj.GetComponent<QuestNPC>();
        npc.AddQuestDialogue(this);
        GameEventsManager.instance.dialogueEvents.onDialogueEnded += DialogueCompleted;
    }

    private void DialogueCompleted(string storyId)
    {
        if(storyId.Equals(story.name))
        {
            GameEventsManager.instance.dialogueEvents.onDialogueEnded -= DialogueCompleted;
            FinishQuestStep();
        }
    }

    private void OnDisable() 
    {
        GameEventsManager.instance.dialogueEvents.onDialogueEnded -= DialogueCompleted;
    }

    public override TextAsset GetQuestStory()
    {
        return story;
    }
}
