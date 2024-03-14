using System;
using Unity;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputEvents : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference tagAction;
    [SerializeField] InputActionReference cancelAction;
    [SerializeField] InputActionReference tabAction;
    public event Action onPlayerInteracted;
    public event Action onTagInteracted;
    public event Action onCancelInteracted;
    public event Action onTabInteracted;
    public void Awake()
    {
        interactAction.action.performed += Interact;
        tagAction.action.performed += TagMenu;
        cancelAction.action.performed += Cancel;
        tabAction.action.performed += TextbookMenu;
    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        if(onPlayerInteracted != null)
        {
            onPlayerInteracted();
        }
    }

    public void TagMenu(InputAction.CallbackContext ctx) {
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

    public void TextbookMenu(InputAction.CallbackContext ctx) {
        if(onTabInteracted!= null)
        {
            onTabInteracted();
        }
    }
}
