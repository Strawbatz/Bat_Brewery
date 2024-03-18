using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SmellSource : MonoBehaviour
{
    [SerializeField] float smellStrength;
    Transform playerTransform;
    SmellManager smellManager;
    bool smelling = false;
    void Start()
    {
        smellManager = GameObject.FindWithTag("Player").GetComponent<SmellManager>();
        playerTransform = GameObject.Find("PlayerFeet").transform;
    }
    
    void Update()
    {
        if(!smelling && Utilities.InCircle(playerTransform.position, transform.position, smellStrength))
        {
            smelling = true;
            smellManager.AddSmell(transform, smellStrength);
        } else if(smelling && !Utilities.InCircle(playerTransform.position, transform.position, smellStrength))
        {
            smelling = false;
            smellManager.RemoveSmell(transform);
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, smellStrength);
    }
    #endif
}
