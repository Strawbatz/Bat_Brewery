using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisionController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] public float maxViewDistance;
    [SerializeField] public float maxClearDistance;
    public Transform GetPlayerTransform(){return playerTransform;}
}
