using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the players movement
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed = 1f;

    [SerializeField] InputActionReference moveAction;

    private Vector2 movementVector = Vector2.zero;

    void Update()
    {
        movementVector = moveAction.action.ReadValue<Vector2>();
        object obj = moveAction.action.ReadValueAsObject();
        Debug.Log(obj);
        transform.position = (Vector2)transform.position + movementVector.normalized*movementSpeed*Time.deltaTime;
    }
}
