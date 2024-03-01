using System;
using Unity;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputEvents : MonoBehaviour
{
    [SerializeField] InputActionReference interactAction;
    public event Action onPlayerInteracted;
    public void Awake()
    {
        interactAction.action.performed += Interact;
    }
    public void Interact(InputAction.CallbackContext ctx)
    {
        if(onPlayerInteracted != null)
        {
            onPlayerInteracted();
        }
    }
}
