using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class changes an object when a quest finishes
/// </summary>
public class ObjectQuestChange : MonoBehaviour
{
    [SerializeField] GameObject beforeQuestObject;
    [SerializeField] GameObject afterQuestObject;
    [SerializeField] QuestInfoSO quest;

    void Start()
    {
        GameEventsManager.instance.questEvents.onFinishQuest += ChangeObject;
    }

    void OnDisable()
    {
        GameEventsManager.instance.questEvents.onFinishQuest -= ChangeObject;
    }

    void ChangeObject(string questId)
    {
        if(quest.id.Equals(questId))
        {
            StartCoroutine(Change());
        }
    }

    IEnumerator Change()
    {
        while (Utilities.InCircle(PlayerManager.instance.GetPlayerFeet().position, transform.position, 8f))
        {
            yield return new WaitForEndOfFrame();
        }

        beforeQuestObject.SetActive(false);
        afterQuestObject.SetActive(true);

        yield return null;
    }
}
