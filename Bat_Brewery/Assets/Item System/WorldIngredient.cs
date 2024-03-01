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
            dialogueManager.choicePicked += Collect;
        }
        
    }

    public void Collect(int choice){
        if(choice == 1) {
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            playerInventory.AddIngredient(itemTag);
            consumed = true;
            gameObject.SetActive(false);
            DialogueManager.GetInstance().choicePicked -= Collect;
        } else {
            DialogueManager.GetInstance().choicePicked -= Collect;
        }
        isTalking = false;
    }
}

