using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempManager : MonoBehaviour
{
    public TaggableItem item;
    [SerializeField] SpriteRenderer box1;
    [SerializeField] SpriteRenderer box2;
    // Start is called before the first frame update
    void Start()
    {
        box1.sprite = item.visualTag.GetInventoryImg();
        box2.sprite = item.visualTag.GetMapImg();
        item.tagUpdated += UpdateItem;
    }

    public void UpdateItem() {
        box1.sprite = item.visualTag.GetInventoryImg();
        box2.sprite = item.visualTag.GetMapImg();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
