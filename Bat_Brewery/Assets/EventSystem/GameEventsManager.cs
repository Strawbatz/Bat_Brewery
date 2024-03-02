using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class should handle all game wide events. 
/// </summary>
public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance {get; private set;}
    public InputEvents inputEvents;
    public QuestEvents questEvents;
    public InventoryEvents inventoryEvents;
    public DialogueEvents dialogueEvents;
    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        } 
        instance = this;

        questEvents = new QuestEvents();
        inventoryEvents = new InventoryEvents();
        dialogueEvents = new DialogueEvents();
    }
}
