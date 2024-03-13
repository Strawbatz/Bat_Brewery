using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

[RequireComponent(typeof(SpriteRenderer))]
public class FogBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float changeDirSpeed;
    [SerializeField] float maxBoxDistance;
    [SerializeField] float fadeSpeed;
    
    SpriteRenderer spriteRenderer;
    Vector2 startPos;
    Vector2 moveDir = Vector2.zero;
    float startAlpha;
    bool resetting;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
        startAlpha = spriteRenderer.color.a;
    }
    void Update()
    {
        if(Utilities.InBox(transform.position, startPos, maxBoxDistance) && !resetting)
        {
            Vector2 changeDir = new Vector2(Random.Range(-1f,1f), Random.Range(-1f,1f));
            moveDir += changeDir/100f*changeDirSpeed;
            moveDir.Normalize();

        transform.Translate(moveDir*speed*Time.deltaTime);
        } else if(!resetting)
        {
            resetting = true;
            MoveToStart();
        } else
        {
            MoveToStart();
        }
    }

    void MoveToStart()
    {
        while (spriteRenderer.color.a > 0 && !((Vector2)transform.position).Equals(startPos))
        {
            Color color = spriteRenderer.color;
            color.a -= fadeSpeed * Time.deltaTime;
            spriteRenderer.color = color;

            return;
        }

        transform.position = startPos;

        while (spriteRenderer.color.a < startAlpha)
        {
            Color color = spriteRenderer.color;
            color.a += fadeSpeed * Time.deltaTime;
            if(color.a > startAlpha) color.a = startAlpha;
            spriteRenderer.color = color;
            return;
        }

        resetting = false;
        return;
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireCube(transform.position, Vector2.one*maxBoxDistance*2);
    }
    #endif
}
