using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.Attributes;
using UnityEngine;

public class TestQuestStep : QuestStep
{   
    [Button("Finish Quest Step")]
    void CompleteQuestStep()
    {
        FinishQuestStep();
    }
}
