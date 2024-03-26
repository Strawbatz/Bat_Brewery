using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] SerializedDictionary<Region, string> regionSceneDictionary;
    public static GameManager instance;
    PlayerManager player; 

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        player = PlayerManager.instance;
    }

    /// <summary>
    /// Change the region and teleport player to respective traversal point
    /// </summary>
    /// <param name="toRegion"></param>
    /// <param name="traversalPoint"></param>
    public void ChangeRegion (Region toRegion, TraversalPointSO traversalPoint)
    {
        if(!regionSceneDictionary.ContainsKey(toRegion))
        {
            Debug.LogWarning("Region does not exist " + toRegion);
            return;
        }
        SceneManager.LoadScene(regionSceneDictionary[toRegion]);

        player.TeleportPlayer(traversalPoint.GetPosition(toRegion));
    }
}

public enum Region
{
    FOREST_MEADOW,
    BEAR_RIVER,
    CRITTERTON,
    BON_BON_FIELDS,
    MUSHROOM_FOREST
}
