using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectRegionCulling : MonoBehaviour
{
    [SerializeField] Vector2 boxSize;
    [SerializeField] Vector2 offset;
    [SerializeField] Color editorColor;

    Transform cameraTransform;
    Vector2 calcFrom;
    void Start()
    {
        calcFrom = (Vector2)transform.position+offset;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if(Utilities.InBox(cameraTransform.position, calcFrom, boxSize))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        } else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = editorColor;
        Handles.DrawWireCube(transform.position+(Vector3)offset, Vector3.one*boxSize);
    }
    #endif
}
