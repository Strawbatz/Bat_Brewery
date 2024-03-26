using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.Attributes;
using UnityEngine;

public class QuestPoint : InteractableObject
{
    [SerializeField] private QuestInfoSO questInfoForPoint;
    private QuestState currentQuestState;
    private string questId;

    [SerializeField] private bool canStart = false;
    [SerializeField] private bool canFinish = false;
    void Awake()
    {
        questId = questInfoForPoint.id;
    }
    void Start()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        currentQuestState = QuestManager.instance.CheckQuestState(questId);
    }
    void OnDisable()
    {
        GameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }
    private void QuestStateChange(Quest quest)
    {
        if(quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
        }
    }
    protected override void Interact()
    {
        Debug.Log(questId + " QM: " + QuestManager.instance.CheckQuestState(questId) + " QP: " + currentQuestState);
        if(currentQuestState.Equals(QuestState.CAN_START) && canStart)
        {
            GameEventsManager.instance.questEvents.StartQuest(questId);
        }
        if(currentQuestState.Equals(QuestState.CAN_FINISH) && canFinish)
        {
            GameEventsManager.instance.questEvents.FinishQuest(questId);
        }
    }
}
