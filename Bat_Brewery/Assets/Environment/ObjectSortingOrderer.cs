using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
/// <summary>
/// This code makes sure that the object is drawn in the correct sorting order in layer compared to the player
/// </summary>
public class ObjectSortingOrderer : MonoBehaviour
{
    [SerializeField]protected float offset;
    protected SpriteRenderer spriteRenderer;

    Transform playerFeet;
    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerFeet = GameObject.Find("PlayerFeet").transform;
    }

    protected virtual void Update()
    {
        if(playerFeet.position.y <= transform.position.y + offset)
        {
            //Draw player on top
            spriteRenderer.sortingOrder = -1;
        } else
        {
            //Draw this object on top
            spriteRenderer.sortingOrder = 1;
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position+Vector3.up*offset, Vector3.forward, .1f);
    }
    #endif
}
