using System.Collections;
using System.Collections.Generic;
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

    private void Start() {
        worldImg.sprite = itemTag.visualTag.GetWorldImg();
        itemTag.tagUpdated += ()=>{worldImg.sprite = itemTag.visualTag.GetWorldImg();}; 
        interactSprite.gameObject.SetActive(false);
        consumed = false;
    }

    protected override void Interact()
    {
        if(consumed == false) {
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            playerInventory.AddIngredient(itemTag);
            consumed = true;
            gameObject.SetActive(false);
        }
        
    }
}

