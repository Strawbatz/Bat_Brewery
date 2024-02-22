using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class mushDood : NPC
{
    public override void Interact(InputAction.CallbackContext ctx)
    {
        Debug.Log("Mushdood dialogue");
    }
}
