using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    /// <summary>
    /// Rotates an object towards another position. The objects x axis will point towards the other position.
    /// </summary>
    /// <param name="objectToRotate"></param>
    /// <param name="targetPosition"></param>
    public static void RotateTowardsPosition(Transform objectToRotate, Vector2 targetPosition, float offsetAngle)
    {
        // Calculate the direction from the object to the target position
        Vector2 direction = targetPosition - (Vector2)objectToRotate.position;

        // Calculate the angle between the direction and the y-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the object to face the target position
        objectToRotate.rotation = Quaternion.Euler(0, 0, angle+offsetAngle);
    }
}
