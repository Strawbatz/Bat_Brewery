using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MushDood : TalkableNPC
{
    public override void Interact(InputAction.CallbackContext ctx)
    {
        Talk(dialogue);
    }


    public override void Talk(TextAsset dialogue)
    {
        DialogueManager.GetInstance().EnterDialogueMode(dialogue, this);
    }
}
