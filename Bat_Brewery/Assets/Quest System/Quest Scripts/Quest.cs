using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.Attributes;
using UnityEngine;
/// <summary>
/// This class represents a quest in the game
/// </summary>
public class Quest
{
    public QuestInfoSO info;
    public QuestState state;

    private int currentQuestStepIndex;

    private List<QuestPrerequisite> questPrerequisites = new List<QuestPrerequisite>();

    public Quest (QuestInfoSO questInfoSO, Transform parentTransform)
    { 
        this.info = questInfoSO;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;

        questPrerequisites = new List<QuestPrerequisite>();
        foreach(QuestPrerequisite prerequisite in questInfoSO.prerequisites)
        {
            QuestPrerequisite newPrerequisite = Object.Instantiate<GameObject>(prerequisite.gameObject, parentTransform).GetComponent<QuestPrerequisite>();
            questPrerequisites.Add(newPrerequisite);
        }
    }

    /// <summary>
    /// Check if all the quest prerequisite are met
    /// </summary>
    /// <returns></returns>
    public bool PrerequisitesMet()
    {
        foreach (QuestPrerequisite prerequisite in questPrerequisites)
        {
            if(!prerequisite.PrerequisiteIsMet()) return false;
        }
        return true;
    }

    /// <summary>
    /// Increases the quest step index
    /// </summary>
    public void MoveToNextStep()
    {
        currentQuestStepIndex ++;
    }

    public bool CurrentStepExist()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    /// <summary>
    /// Instantiates the current quest step
    /// </summary>
    /// <param name="parentTransform"></param>
    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id);
        }
    }

    /// <summary>
    /// Removes quest prerequisites
    /// </summary>
    public void CleanPrerequisites()
    {
        foreach(QuestPrerequisite prerequisite in questPrerequisites)
        {
            GameObject.Destroy(prerequisite.gameObject);
        }
        questPrerequisites = new List<QuestPrerequisite>();
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if(CurrentStepExist())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        } else
        {
            Debug.LogError("Quest step " + currentQuestStepIndex + " doesn't exist in " + info.id);
        }

        return questStepPrefab;
    }
}
