using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VisualTag : ScriptableObject
{
   [SerializeField] private Sprite inventoryImg;
   [SerializeField] private Sprite mapImg;

    public Sprite GetInventoryImg() {
        return inventoryImg;
    }
    public Sprite GetMapImg() {
        return mapImg;
    }
}


