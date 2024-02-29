using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Abstract class enabling interaction. Needs a triggerbox to work.
/// </summary>
public abstract class NPC : MonoBehaviour, IInteractable
{
[SerializeField] InputActionReference interact;
[SerializeField] public SpriteRenderer interactSprite;

/// <summary>
/// If player enters triggerbox, display interact indicator and listen for input
/// from the interact key.
/// </summary>
/// <param name="other"></param>
private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.CompareTag("PlayerPhysics")) {
        interact.action.performed += Interact;
        interactSprite.gameObject.SetActive(true);
    }
}

/// <summary>
/// If player exits triggerbox, hide interaction indicator and stop
/// listening for input from interact key.
/// </summary>
/// <param name="other"></param>
private void OnTriggerExit2D(Collider2D other) {
    if(other.gameObject.CompareTag("PlayerPhysics")) {
        interact.action.performed -= Interact;
        interactSprite.gameObject.SetActive(false);
    }
}

/// <summary>
/// Abstract function to make sure inherited class has a function for 
/// when interact is pressed.
/// </summary>
/// <param name="ctx"></param>
public abstract void Interact(InputAction.CallbackContext ctx);
}
