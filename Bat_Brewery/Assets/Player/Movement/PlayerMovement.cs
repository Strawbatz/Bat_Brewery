using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Controls the players movement
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    [SerializeField] InputActionReference moveAction;

    private Vector2 movementVector = Vector2.zero;

    [SerializeField] private Animator animator;
    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        movementVector = moveAction.action.ReadValue<Vector2>();
        object obj = moveAction.action.ReadValueAsObject();

        //transform.position = (Vector2)transform.position + movementVector.normalized*movementSpeed*Time.deltaTime;

        rigidbody.velocity = movementVector.normalized*movementSpeed*Time.fixedDeltaTime;


        animator.SetFloat("Horizontal_M", movementVector.x);
        animator.SetFloat("Vertical_M", movementVector.y);
    }
}
