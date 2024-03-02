using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.Attributes;
using UnityEngine;
public class Quest
{
    public QuestInfoSO info;
    public QuestState state;

    private int currentQuestStepIndex;

    public Quest (QuestInfoSO questInfoSO)
    { 
        this.info = questInfoSO;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex ++;
    }

    public bool CurrentStepExist()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if(questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id);
        }
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
