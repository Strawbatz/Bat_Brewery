using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class places and controls the footsteps created by the player 
/// </summary> 
public class FootstepController : MonoBehaviour
{
    [SerializeField] private Transform playerFeet;
    [SerializeField] private GameObject[] footstepPrefab;
    [SerializeField] int maximumFootsteps;
    [SerializeField] float footstepDistance;
    private Queue<GameObject> playerFootsteps = new Queue<GameObject>();
    private Stack<GameObject> footstepPool = new Stack<GameObject>();
    private Vector2 lastPosition;

    void Start()
    {
        lastPosition = playerFeet.position;
        for (int i = 0; i < transform.childCount; i++)
        {
            footstepPool.Push(transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        float currentDist = Vector2.Distance(lastPosition, (Vector2)playerFeet.position);

        if(currentDist > footstepDistance)
        {
            CreateFootstep();
            if(playerFootsteps.Count > maximumFootsteps)
            {
                footstepPool.Push(playerFootsteps.Dequeue());
            }
        }

        foreach (GameObject footstep in playerFootsteps)
        {
            if(Utilities.InRange(footstep.transform.position, (Vector2)playerFeet.parent.position-Vector2.one*footstepDistance, 
            (Vector2)playerFeet.parent.position+Vector2.one*footstepDistance))
            {
                if(Vector2.Distance(footstep.transform.position, playerFeet.parent.position) < footstepDistance)
                {
                    footstep.SetActive(false);
                    continue;
                } 
            } 
            footstep.SetActive(true);
        }
    }


    private void CreateFootstep()
    {
        GameObject footstep;
        if(footstepPool.Count <= 0)
        {
            footstep = GameObject.Instantiate(footstepPrefab[UnityEngine.Random.Range(0,footstepPrefab.Length)], transform) as GameObject;
            footstep.SetActive(false);
        } else
        {
            footstep = footstepPool.Pop();
        }

        footstep.transform.position = lastPosition;
        Utilities.RotateTowardsPosition(footstep.transform, playerFeet.transform.position,-90);
        footstep.SetActive(true);
        playerFootsteps.Enqueue(footstep);

        lastPosition = playerFeet.position;
    }
}
