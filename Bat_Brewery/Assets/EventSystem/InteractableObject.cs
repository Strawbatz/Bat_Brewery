using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Abstract class enabling interaction. Needs a triggerbox to work.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public abstract class InteractableObject : MonoBehaviour
{
[SerializeField] public SpriteRenderer interactSprite;

/// <summary>
/// If player enters triggerbox, display interact indicator and listen for input
/// from the interact key.
/// </summary>
/// <param name="other"></param>
protected virtual void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.CompareTag("PlayerPhysics")) {
        GameEventsManager.instance.inputEvents.onPlayerInteracted += Interact;
        if(interactSprite) interactSprite.gameObject.SetActive(true);
    }
}

/// <summary>
/// If player exits triggerbox, hide interaction indicator and stop
/// listening for input from interact key.
/// </summary>
/// <param name="other"></param>
protected virtual void OnTriggerExit2D(Collider2D other) {
    if(other.gameObject.CompareTag("PlayerPhysics")) {
        GameEventsManager.instance.inputEvents.onPlayerInteracted -= Interact;
        if(interactSprite) interactSprite.gameObject.SetActive(false);
    }
}

/// <summary>
/// Abstract function to make sure inherited class has a function for 
/// when interact is pressed.
/// </summary>
protected abstract void Interact();
}
