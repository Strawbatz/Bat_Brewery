using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.Attributes;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class QuestPoint : MonoBehaviour
{
    [SerializeField] private QuestInfoSO questInfoForPoint;
    [SerializeField, ReadOnly]private bool playerIsNear = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerPhysics")) playerIsNear = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerPhysics")) playerIsNear = false;
    }
}
