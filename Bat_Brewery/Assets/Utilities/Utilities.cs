using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
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

    /// <summary>
    /// Returns true if the value is inside of the specified range
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static bool InRange(float value, float minValue, float maxValue)
    {
        if(value > maxValue) return false;
        if(value < minValue) return false;
        return true;
    }

    /// <summary>
    /// Returns true if the vector is inside of the bounds represented by the other vectors
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="maxVector"></param>
    /// <param name="minVector"></param>
    /// <returns></returns>
    public static bool InRange(Vector2 vector, Vector2 minVector, Vector2 maxVector)
    {
        if(!InRange(vector.x, minVector.x, maxVector.x)) return false;
        if(!InRange(vector.y, minVector.y, maxVector.y)) return false;
        return true;
    }
}
