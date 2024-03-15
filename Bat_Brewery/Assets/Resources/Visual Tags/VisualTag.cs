using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VisualTag : ScriptableObject
{
    //List containing all visual versions of a tag.
   [SerializeField] private Sprite inventoryImg;
   [SerializeField] private Sprite inventoryImg2;
   [SerializeField] private Sprite mapImg;
   [SerializeField] private Sprite worldImg;
   [SerializeField] private Sprite harvestedImg;

    //List of getters for versions of visual tag.
    public Sprite GetInventoryImg() {
        return inventoryImg;
    }
    public Sprite GetInventoryImg2() {
        return inventoryImg2;
    }
    /// <summary>
    /// Returns one of the 2 inventory images that exists at random.
    /// </summary>
    /// <returns></returns>
    public Sprite GetRandomInventoryImg() {
        if(Random.Range(0,1) == 0) {
            return inventoryImg;
        } else {
            return inventoryImg2;
        }
    }
    public Sprite GetMapImg() {
        return mapImg;
    }
    public Sprite GetWorldImg() {
        return worldImg;
    }

    public Sprite GetHarvestedImg()
    {
        return harvestedImg;
    }
}


