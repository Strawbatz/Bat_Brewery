using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.Attributes;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class TraversalPointSO : ScriptableObject
{
    [SerializeField] SerializedDictionary<Region, Vector2> traversalPoints;

    public Vector2 GetPosition(Region toRegion)
    {
        if(!traversalPoints.ContainsKey(toRegion))
        {
            Debug.LogError("Traversal point doesn't contain Region");
            return Vector2.zero;
        }

        return traversalPoints[toRegion];
    }
}
