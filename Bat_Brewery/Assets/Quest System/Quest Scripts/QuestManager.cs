using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AdvancedEditorTools.Attributes;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    private Dictionary<string, Quest> questMap;
    
    [SerializeField, ReadOnly] private string[]viewQuests =  new string[0];

    void Awake()
    {
        if(instance) 
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        if(questMap == null)
            questMap = CreateQuestMap();
        viewQuests = questMap.Keys.ToArray<string>();
    }

    void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }
    void Start()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
        //Broadcast the initial state of all quests on startup
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        if(quest.state == QuestState.REQUIREMENTS_NOT_MET && state == QuestState.CAN_START) 
        {
            quest.CleanPrerequisites();
        } 
        quest.state = state;
    
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if(GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                return false;
            }
        }
        return quest.PrerequisitesMet();
    }

    void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if(quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest (string id)
    {
        Debug.Log("Quest " + id + " started");
        
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("Quest " + id + " advanced");
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();

        if(quest.CurrentStepExist())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        } else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Debug.Log("Quest " + id + " finished");
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        //TODO: IMPLEMENT THIS
    }
    private Dictionary<string, Quest> CreateQuestMap ()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if(idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo, this.transform));
        }

        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if(quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    public QuestState CheckQuestState(string id)
    {
        Quest quest = GetQuestById(id);
        return quest.state;
    }
}
