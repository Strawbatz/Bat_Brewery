using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// Script on ingredients found in the world. They are interactable
/// and can be picked up and put into player inventory. It will then be consumed
/// until scene is loaded again. You can also change it's visual representation
/// by opening the tagMenu and selecting a new visual representation.
/// </summary>
public class WorldIngredient : InteractableObject
{
    
    [SerializeField] public Ingredient itemTag;
    [SerializeField] private SpriteRenderer worldImg;
    [SerializeField] private TextAsset interactText;
    [SerializeField] private TextAsset consumedText;
    private bool isTalking;
    private PickupMode mode = PickupMode.UNKNOWN;

    private void Start() {
        worldImg.sprite = itemTag.visualTag.GetWorldImg();
        itemTag.tagUpdated += ()=>{worldImg.sprite = itemTag.visualTag.GetWorldImg();}; 
        
        interactSprite.gameObject.SetActive(false);
        gameObject.SetActive(true);
        mode = PickupMode.UNKNOWN; //TODO Load from save
        isTalking = false;
    }

    void OnDisable()
    {
        GameEventsManager.instance.dialogueEvents.onDialogueEnded -= DescriptionEnded;
        GameEventsManager.instance.dialogueEvents.onDialogueEnded -= RedoDescription;
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= InteractChoice;
    }

    protected override void Interact()
    {
        if(!isTalking && !ItemTagManager.instance.isOpen)
        {
            isTalking = true;
            switch(mode)
            {
                case PickupMode.UNKNOWN:
                {
                    DialogueManager.GetInstance().EnterDescription(itemTag.description);
                    GameEventsManager.instance.dialogueEvents.onDialogueEnded += DescriptionEnded;
                    mode = PickupMode.DISCOVERED;
                    return;     
                }
                case PickupMode.DISCOVERED:
                {
                    DialogueManager.GetInstance().EnterDescription(interactText);
                    GameEventsManager.instance.dialogueEvents.onChoiceMade += InteractChoice;
                    return;
                }
                case PickupMode.HARVESTED:
                {
                    DialogueManager.GetInstance().EnterDescription(consumedText);
                    GameEventsManager.instance.dialogueEvents.onChoiceMade += InteractChoice;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Is called when the description of the ingredient ends
    /// </summary>
    /// <param name="id"></param>
    private void DescriptionEnded(string id)
    {
        if(id.Equals(itemTag.description.name))
        {
            if(mode == PickupMode.HARVESTED) DialogueManager.GetInstance().EnterDescription(consumedText);
            else DialogueManager.GetInstance().EnterDescription(interactText);
            GameEventsManager.instance.dialogueEvents.onDialogueEnded -= DescriptionEnded;
            GameEventsManager.instance.dialogueEvents.onChoiceMade += InteractChoice;
        }
    }

    /// <summary>
    /// Is called when the player chooses to investigate the ingredient in the ingredient choice
    /// </summary>
    /// <param name="id"></param>
    private void RedoDescription(string id)
    {
        if(!id.Equals(interactText.name) && !id.Equals(consumedText.name)) return;
        if(!isTalking && !ItemTagManager.instance.isOpen)
        {
            isTalking = true;
            DialogueManager.GetInstance().EnterDescription(itemTag.description);
            GameEventsManager.instance.dialogueEvents.onDialogueEnded += DescriptionEnded;
        }

        GameEventsManager.instance.dialogueEvents.onDialogueEnded -= RedoDescription;
    }

    private void InteractChoice(string storyId, int choice){
        if(storyId.Equals(interactText.name)){
            switch(choice)
            {
                case 0:
                {
                    //The player picks up the item
                    HarvestPlant();
                    break;
                }
                case 1:
                {
                    //The player investigates the ingredient
                    GameEventsManager.instance.dialogueEvents.onDialogueEnded += RedoDescription;
                    break;
                }
                case 2:
                {
                    //The player changes visual tag of item
                    ItemTagManager.instance.ToggleMenu(itemTag);
                    break;
                }
            }
        } else if(storyId.Equals(consumedText.name))
        {
            switch(choice)
            {
                case 0:
                {
                    //The player investigates the ingredient
                    GameEventsManager.instance.dialogueEvents.onDialogueEnded += RedoDescription;
                    break;
                }
                case 1:
                {
                    //The player changes visual tag of item
                    ItemTagManager.instance.ToggleMenu(itemTag);
                    break;
                }
            }
        }

        GameEventsManager.instance.dialogueEvents.onChoiceMade -= InteractChoice;
        isTalking = false;
        return;
    }

    /// <summary>
    /// Harvests this plant
    /// </summary>
    public void HarvestPlant()
    {
        GameEventsManager.instance.inventoryEvents.PickUpIngredient(itemTag);
        mode = PickupMode.HARVESTED;
        worldImg.sprite = itemTag.visualTag.GetHarvestedImg();
    }

    /// <summary>
    /// Regrow this plant
    /// </summary>
    public void RegrowPlant()
    {
        mode = PickupMode.DISCOVERED;
        worldImg.sprite = itemTag.visualTag.GetWorldImg();
    }

    private void TagMenuToggle() {
        if(!isTalking) { 
        ItemTagManager.instance.ToggleMenu(itemTag);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other) {
        base.OnTriggerEnter2D(other);
        if(other.gameObject.CompareTag("PlayerPhysics")) {
            GameEventsManager.instance.inputEvents.onTagInteracted += TagMenuToggle;
            GameEventsManager.instance.inputEvents.onCancelInteracted += ItemTagManager.instance.ExitMenu;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other) {
        base.OnTriggerExit2D(other);
        if(other.gameObject.CompareTag("PlayerPhysics")) {
            GameEventsManager.instance.inputEvents.onTagInteracted -= TagMenuToggle;
            GameEventsManager.instance.inputEvents.onCancelInteracted -= ItemTagManager.instance.ExitMenu;
            if(ItemTagManager.instance.isOpen) {
                ItemTagManager.instance.ExitMenu();
            }
        }
    }

    void OnValidate()
    {
        worldImg.sprite = itemTag.visualTag.GetWorldImg();
    }

    private enum PickupMode
    {
        //The player has never interacted with this item pickup before
        UNKNOWN,

        //The player has interacted with this item pickup before
        DISCOVERED,

        //This item pickup is harvested
        HARVESTED
    }
}

