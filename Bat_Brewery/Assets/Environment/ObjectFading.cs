using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fades an object that is far away from the player
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ObjectFading : MonoBehaviour
{
    private Transform player;
    private PlayerVisionController visionController;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        visionController = FindObjectOfType<PlayerVisionController>();
        player = visionController.GetPlayerTransform();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = Color.white;
        color.a = 0;
        spriteRenderer.color = color;
    }

    void Update()
    {
        if(Utilities.InBox(transform.position, player.position, visionController.maxViewDistance+2))
        {
            float dist = Vector2.Distance(transform.position, player.position);
            float alpha = 0;
            Color color = Color.white;
            if(dist < visionController.maxViewDistance)
            {
                alpha = 1-((dist-visionController.maxClearDistance)/(visionController.maxViewDistance-visionController.maxClearDistance));
                if(alpha > 1) alpha = 1;
                else if(alpha < 0) alpha = 0;
            } 
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
