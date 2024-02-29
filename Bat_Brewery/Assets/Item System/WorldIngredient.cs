using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Script on ingredients found in the world. They are interactable
/// and can be picked up and put into player inventory. It will then be consumed
/// until scene is loaded again.
/// </summary>
public class WorldIngredient : NPC
{
    
    [SerializeField] public TaggableItem itemTag;
    [SerializeField] private SpriteRenderer worldImg;
    [SerializeField] public IngredientType ingredientType;
    private bool consumed;

    private void Start() {
        worldImg.sprite = itemTag.visualTag.GetWorldImg();
        interactSprite.gameObject.SetActive(false);
        consumed = false;
    }

    public override void Interact(InputAction.CallbackContext ctx)
    {
        if(consumed == false) {
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            playerInventory.AddIngredient(this);
            consumed = true;
            this.gameObject.SetActive(false);
        }
        
    }
}

