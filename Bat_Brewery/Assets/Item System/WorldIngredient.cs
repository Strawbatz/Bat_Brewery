using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


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
    private bool consumed;
    private bool isTalking;

    private bool firstTime = false;

    private void Start() {
        worldImg.sprite = itemTag.visualTag.GetWorldImg();
        itemTag.tagUpdated += ()=>{worldImg.sprite = itemTag.visualTag.GetWorldImg();}; 
        
        interactSprite.gameObject.SetActive(false);
        gameObject.SetActive(true);
        consumed = false;
        isTalking = false;
    }

    protected override void Interact()
    {
        if(!consumed && !isTalking && !ItemTagManager.instance.isOpen) {
            isTalking = true;
            DialogueManager dialogueManager = DialogueManager.GetInstance();
            dialogueManager.EnterDescription(itemTag.description);
            GameEventsManager.instance.dialogueEvents.onChoiceMade += Collect;
        }
    }

    public void Collect(string storyId, int choice){
        if(!storyId.Equals(itemTag.description.name)) return;
        if(choice == 0) {
            GameEventsManager.instance.inventoryEvents.PickUpIngredient(itemTag);
            consumed = true;
            gameObject.SetActive(false);
        }
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= Collect;
        isTalking = false;
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
}

