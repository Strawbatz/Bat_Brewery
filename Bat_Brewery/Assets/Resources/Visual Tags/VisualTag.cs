using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VisualTag : ScriptableObject
{
    //List containing all visual versions of a tag.
   [SerializeField] private Sprite inventoryImg;
   [SerializeField] private Sprite mapImg;

    //List of getters for versions of visual tag.
    public Sprite GetInventoryImg() {
        return inventoryImg;
    }
    public Sprite GetMapImg() {
        return mapImg;
    }
}


