using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;
    Dictionary<string, int> playerChoicesMade = new Dictionary<string, int>();

    void Awake()
    {
        if(instance)
        {
            Debug.LogWarning("A Story Manager already exists in this scene");
        }

        instance = this;
    }

    /// <summary>
    /// Return the choice made for a specific question
    /// </summary>
    /// <param name="storyId"></param>
    /// <returns></returns>
    public int ChoiceMadeFor(string storyId)
    {
        if(playerChoicesMade.ContainsKey(storyId))
        {
            return playerChoicesMade[storyId];
        }

        return -1;
    }
}
