using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement of the graphical part of the player
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerGraphicsMovement : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Transform feetTransform;
    [SerializeField] float speed;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 targetPos = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
        Vector3 moveVector = Vector3.Slerp(transform.position, targetPos,speed*Time.deltaTime);
        transform.position = moveVector;
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(feetTransform.position.y*100);
    }
}
