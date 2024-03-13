using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class ObjectObstructionFading : ObjectFading
{
    [SerializeField] float obstructionFade = 0.2f;
    [SerializeField] float fadeSpeed = 1f;

    bool fading = false;
    int fadeDir = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerPhysics"))
        {
            fadeDir = -1;
            StartCoroutine(Fade());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("PlayerPhysics"))
        {
            fadeDir = 1;
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        if(fading) yield return null;
        fading = true;
        while((maxAlpha > obstructionFade && fadeDir < 0) || (maxAlpha < 1f && fadeDir > 0))
        {
            Debug.Log("maxAlpha " + maxAlpha + " : Sprite colorA " + spriteRenderer.color.a);
            maxAlpha += fadeSpeed*Time.deltaTime*fadeDir;
            yield return new WaitForEndOfFrame();
        }
        fading = false;
        yield return null;
    }
}
