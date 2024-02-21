using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement of the graphical part of the player
/// </summary>
public class PlayerGraphicsMovement : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Transform feetTransform;
    [SerializeField] float cameraSpeed;

    void Update()
    {
        Vector3 targetPos = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
        Vector3 moveVector = Vector3.Slerp(transform.position, targetPos,cameraSpeed*Time.deltaTime);
        transform.position = moveVector;
    }
}
