using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
[SerializeField] InputAction interact;
[SerializeField] SpriteRenderer interactSprite;

private void Start() {
    interactSprite.gameObject.SetActive(false);
}

private void OnTriggerEnter2D(Collider2D other) {
    if(other.gameObject.CompareTag("Player")) {
        interact.performed += Interact;
        interactSprite.gameObject.SetActive(true);
        Debug.Log("Entered");
    }else Debug.Log("hello");
}

private void OnTriggerExit2D(Collider2D other) {
    if(other.gameObject.CompareTag("Player")) {
        interact.performed -= Interact;
        interactSprite.gameObject.SetActive(true);
        Debug.Log("Exited");
    } else Debug.Log("hello");
}

    public abstract void Interact(InputAction.CallbackContext ctx);
}
