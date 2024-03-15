using System;
using Unity;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputEvents : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference tagAction;
    [SerializeField] InputActionReference cancelAction;
    public event Action onPlayerInteracted;
    public event Action onTagInteracted;
    public event Action onCancelInteracted;
    public void Awake()
    {
        interactAction.action.performed += Interact;
        tagAction.action.performed += TagMenu;
        cancelAction.action.performed += Cancel;
    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        if(onPlayerInteracted != null)
        {
            onPlayerInteracted();
        }
    }

    public void TagMenu(InputAction.CallbackContext ctx) {
        TagMenu();
    }
    public void TagMenu() {
        if(onTagInteracted != null)
        {
            onTagInteracted();
        }
    }

    public void Cancel(InputAction.CallbackContext ctx) {
        if(onCancelInteracted!= null)
        {
            onCancelInteracted();
        }
    }
}
