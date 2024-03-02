using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedEditorTools.Attributes;

[CreateAssetMenu(fileName = "NewQuestInfoSO", menuName = "Quest/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field:SerializeField, ReadOnly] public string id {get; private set;}
    
    public string displayName;
    public QuestInfoSO[] questPrerequisites;
    public GameObject[] questStepPrefabs;

    //TODO: Add rewards

    //ensure the id is always the name of the Scriptable Object asset
    private void OnValidate() 
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
