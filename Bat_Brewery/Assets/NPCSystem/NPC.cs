using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
[SerializeField] InputActionReference interact;
[SerializeField] SpriteRenderer interactSprite;

private void Start() {
    interactSprite.gameObject.SetActive(false);
}

private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.CompareTag("PlayerPhysics")) {
        interact.action.performed += Interact;
        interactSprite.gameObject.SetActive(true);
    }
}

private void OnTriggerExit2D(Collider2D other) {
    if(other.gameObject.CompareTag("PlayerPhysics")) {
        interact.action.performed -= Interact;
        interactSprite.gameObject.SetActive(false);
    }
}

    public abstract void Interact(InputAction.CallbackContext ctx);
}
