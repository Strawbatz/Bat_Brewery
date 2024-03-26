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
    public PlayerMovementEvents playerMovementEvents;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        } 
        instance = this;

        questEvents = new QuestEvents();
        inventoryEvents = new InventoryEvents();
        dialogueEvents = new DialogueEvents();
        playerMovementEvents = new PlayerMovementEvents();
    }
}
