using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [SerializeField, AdvancedEditorTools.Attributes.ReadOnly] private string id;
    public string GetId()
    {
        return id;
    }

    private void OnValidate() 
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
