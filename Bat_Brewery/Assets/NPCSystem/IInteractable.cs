using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Interface dictating if something is interactable.
/// </summary>
public interface IInteractable
{
    public void Interact(InputAction.CallbackContext ctx);
}
