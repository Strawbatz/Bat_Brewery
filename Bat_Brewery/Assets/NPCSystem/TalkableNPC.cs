using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TalkableNPC : NPC
{
    [SerializeField] public String NPCName;
    [SerializeField] public Sprite portrait;
    [SerializeField] public TextAsset dialogue;

    public abstract void Talk(TextAsset dialogue);
}
