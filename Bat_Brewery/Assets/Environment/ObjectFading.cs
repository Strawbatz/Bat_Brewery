using System.Collections;
using System.Collections.Generic;
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

    protected override void Update()
    {
        base.Update();
        Vector3 position = transform.position + Vector3.up*offset;
        if(Utilities.InBox(position, player.position, visionController.maxViewDistance*fadeStrength+2))
        {
            float dist = Vector2.Distance(position, player.position);
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
