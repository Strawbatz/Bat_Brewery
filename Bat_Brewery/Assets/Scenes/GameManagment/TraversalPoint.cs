using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TraversalPoint : InteractableObject
{
    [SerializeField] Region toRegion;
    [SerializeField] TraversalPointSO traversalPointSO;
    protected override void Interact()
    {
        GameManager.instance.ChangeRegion(toRegion, traversalPointSO);
    }
}
