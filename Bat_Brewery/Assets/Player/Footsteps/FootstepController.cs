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
    [SerializeField] private GameObject[] footstepPrefab;
    [SerializeField] int maximumFootsteps;
    [SerializeField] float footstepDistance;
    [SerializeField] float playerNoFootstepRadius;
    [SerializeField] float playerFadeFootstepsRadius;
    private Queue<GameObject> playerFootsteps = new Queue<GameObject>();
    private Stack<GameObject> footstepPool = new Stack<GameObject>();
    private Vector2 lastPosition;

    void Start()
    {
        lastPosition = PlayerManager.instance.GetPlayerFeet().position;
        for (int i = 0; i < transform.childCount; i++)
        {
            footstepPool.Push(transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        float currentDist = Vector2.Distance(lastPosition, (Vector2)PlayerManager.instance.GetPlayerFeet().position);

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
            if(Utilities.InRange(footstep.transform.position, (Vector2)PlayerManager.instance.GetPlayerFeet().parent.position-Vector2.one*(playerFadeFootstepsRadius+2), 
            (Vector2)PlayerManager.instance.GetPlayerFeet().parent.position+Vector2.one*(playerFadeFootstepsRadius+2)))
            {
                float dist = Vector2.Distance(footstep.transform.position, PlayerManager.instance.GetPlayerFeet().parent.position);
                if(dist < playerFadeFootstepsRadius)
                {
                    Color color = Color.white;
                    color.a = (dist - playerNoFootstepRadius)/(playerFadeFootstepsRadius-playerNoFootstepRadius);
                    footstep.GetComponent<SpriteRenderer>().color = color;
                    continue;
                } else
                {
                    footstep.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
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
        Utilities.RotateTowardsPosition(footstep.transform, PlayerManager.instance.GetPlayerFeet().transform.position,-90);
        footstep.SetActive(true);
        playerFootsteps.Enqueue(footstep);

        lastPosition = PlayerManager.instance.GetPlayerFeet().position;
    }
}
