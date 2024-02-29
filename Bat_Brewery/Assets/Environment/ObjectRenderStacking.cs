using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRenderStacking : MonoBehaviour
{

    void Start()
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("Utilities/RenderStackingPrefab") as GameObject,transform);
        obj.transform.position = transform.position;
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
    }
}
