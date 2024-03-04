using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class containing components needed for dialogue.
/// </summary>
public abstract class TalkableNPC : InteractableObject
{
    //Name to be displayed.
    [SerializeField] public String npcName;

    //Portrait to be shown in dialogue.
    [SerializeField] public Sprite portrait;

    //JSON file generated from inky containing dialogue. 
    [SerializeField] public TextAsset defaultDialogue;

    /// <summary>
    /// abstract function to make sure a talk function exists.
    /// It takes in a JSON generated by inky for dialogue.
    /// </summary>
    /// <param name="dialogue">JSON file generated by inky</param>
    public abstract void Talk(TextAsset dialogue);
}
