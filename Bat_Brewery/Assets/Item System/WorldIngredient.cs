using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script on ingredients found in the world. They are interactable
/// and can be picked up and put into player inventory. It will then be consumed
/// until scene is loaded again.
/// </summary>
public class WorldIngredient : InteractableObject
{
    
    [SerializeField] public Ingredient itemTag;
    [SerializeField] private SpriteRenderer worldImg;
    private bool consumed;
    private bool isTalking;

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
        if(!consumed && !isTalking) {
            isTalking = true;
            DialogueManager dialogueManager = DialogueManager.GetInstance();
            dialogueManager.EnterDescription(itemTag.description);
            GameEventsManager.instance.dialogueEvents.onChoiceMade += Collect;
        }
    }

    public void Collect(string storyId, int choice){
        if(!storyId.Equals(itemTag.description.name)) return;
        if(choice == 1) {
            GameEventsManager.instance.inventoryEvents.PickUpIngredient(itemTag);
            consumed = true;
            gameObject.SetActive(false);
        }
        GameEventsManager.instance.dialogueEvents.onChoiceMade -= Collect;
        isTalking = false;
    }
}

