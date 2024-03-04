using System;
using UnityEngine;

public class DialogueEvents
{
    /// <summary>
    /// Is fired when a dialogue has started. Contains the id if the story that was started.
    /// </summary>
    public event Action<string> onDialogueStarted;
    public void DialogueStarted(string storyId)
    {
        if(onDialogueStarted != null)
        {
            onDialogueStarted(storyId);
        }
    }


    /// <summary>
    /// Is fired when a dialogue has ended. Contains the id of the story that was ended.
    /// </summary>
    public event Action<string> onDialogueEnded;
    public void DialogueEnded(string storyId)
    {
        if(onDialogueEnded != null)
        {
            onDialogueEnded(storyId);
        }
    }

    /// <summary>
    /// Is fired when a choice in a dialogue has been made. Contains the id of the story that the choice was made in and the index for the choice made.
    /// </summary>
    public event Action<string, int> onChoiceMade;
    public void ChoiceMade(string storyId, int choice)
    {
        if(onChoiceMade != null)
        {
            onChoiceMade(storyId, choice);
        }
    }
}
