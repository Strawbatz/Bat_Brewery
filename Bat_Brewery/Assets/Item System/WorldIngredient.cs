using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldIngredient : NPC
{
    
    [SerializeField] public TaggableItem itemType;
    [SerializeField] private SpriteRenderer worldImg;

    private void Start() {
        worldImg.sprite = itemType.visualTag.GetWorldImg();
        interactSprite.gameObject.SetActive(false);
    }

    public override void Interact(InputAction.CallbackContext ctx)
    {
        PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        playerInventory.AddIngredient(this);
    }
}

