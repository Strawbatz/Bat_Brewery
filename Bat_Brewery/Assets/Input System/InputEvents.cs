using System;
using Unity;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputEvents : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    [SerializeField] InputActionReference tagAction;
    public event Action onPlayerInteracted;
    public event Action onTagInteracted;
    public void Awake()
    {
        interactAction.action.performed += Interact;
        tagAction.action.performed += tagMenu;
    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        if(onPlayerInteracted != null)
        {
            onPlayerInteracted();
        }
    }

    public void tagMenu(InputAction.CallbackContext ctx) {
        if(onTagInteracted != null)
        {
            onTagInteracted();
        }
    }
}
