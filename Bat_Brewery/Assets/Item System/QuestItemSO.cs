using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object representing Quest Items.
/// </summary>
public class QuestItemSO : ItemSO
{
    [SerializeField] public Sprite Image;
    [TextArea(5, 10)]
    [SerializeField] public string description;
}
