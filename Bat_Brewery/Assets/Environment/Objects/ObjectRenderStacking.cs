using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectRenderStacking : MonoBehaviour
{
    void Start()
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("Utilities/RenderStackingPrefab") as GameObject,transform);
        obj.transform.position = transform.position;
        SpriteRenderer stackingRenderer = obj.GetComponent<SpriteRenderer>();
        stackingRenderer.sprite = GetComponent<SpriteRenderer>().sprite;
        StartCoroutine(Setup(stackingRenderer));
    }
    IEnumerator Setup(SpriteRenderer stackingRenderer)
    {
        yield return new WaitForEndOfFrame();
        stackingRenderer.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
    }
}
