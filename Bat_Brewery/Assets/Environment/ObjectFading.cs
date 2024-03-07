using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TreeEditor;
using UnityEngine;

/// <summary>
/// Fades an object that is far away from the player
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ObjectFading : ObjectSortingOrderer
{
    [SerializeField] float fadeStrength = 1f;
    private Transform player;
    private PlayerVisionController visionController;

    protected float maxAlpha = 1;

    protected override void Start()
    {
        base.Start();
        visionController = FindObjectOfType<PlayerVisionController>();
        player = visionController.GetPlayerTransform();
        Color color = Color.white;
        color.a = 0;
        spriteRenderer.color = color;
    }

    void Update()
    {
        Vector3 positionOff = transform.position + Vector3.up*offset;
        Vector3 position = transform.position;
        if(Utilities.InBox(positionOff, player.position, visionController.maxViewDistance*fadeStrength+2) || 
            Utilities.InBox(position, player.position, visionController.maxViewDistance*fadeStrength+2))
        {
            float distPos = Vector2.Distance(position, player.position);
            float distOff = Vector2.Distance(positionOff, player.position);
            float dist = (distOff>distPos)?distPos:distOff;
            float alpha = 0;
            Color color = Color.white;
            if(dist < visionController.maxViewDistance*fadeStrength)
            {
                alpha = (1-((dist-visionController.maxClearDistance*fadeStrength)/(visionController.maxViewDistance*fadeStrength-visionController.maxClearDistance*fadeStrength)))*maxAlpha;
                if(alpha > 1) alpha = maxAlpha;
                else if(alpha < 0) alpha = 0;
            } 
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
