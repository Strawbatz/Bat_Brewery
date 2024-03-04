using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ObjectObstructionFading : ObjectFading
{
    [SerializeField] float obstructionFade = 0.1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerPhysics"))
        {
            maxAlpha = obstructionFade;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerPhysics"))
        {
            maxAlpha = 1f;
        }
    }
}
