using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Test script for creating an npc with dialogue.
/// </summary>
public class MushDood : TalkableNPC
{
    public override void Interact()
    {
        Talk(dialogue);
    }


    public override void Talk(TextAsset dialogue)
    {
        DialogueManager.GetInstance().EnterDialogueMode(dialogue, this);
    }
}
