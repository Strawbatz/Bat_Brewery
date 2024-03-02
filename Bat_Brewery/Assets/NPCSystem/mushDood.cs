using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Test script for creating an npc with dialogue.
/// </summary>
public class MushDood : TalkableNPC
{
    protected override void Interact()
    {
        Talk(defaultDialogue);
    }


    public override void Talk(TextAsset dialogue)
    {
        DialogueManager.GetInstance().EnterDialogueMode(dialogue, this);
    }
}
