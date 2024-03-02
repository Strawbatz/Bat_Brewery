using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQS : QuestStep
{
    [SerializeField] TextAsset story;
    [SerializeField] string npcName;

    QuestNPC npc;
    private void Start()
    {
        GameObject obj = GameObject.Find(npcName);
        if(!obj) Debug.LogError("The NPC with name " + npcName + " could not be found");

        npc = obj.GetComponent<QuestNPC>();
        npc.AddQuestDialogue(story);
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
}
