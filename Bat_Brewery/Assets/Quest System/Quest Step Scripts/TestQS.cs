using System.Collections;
using System.Collections.Generic;
using AdvancedEditorTools.Attributes;
using UnityEngine;

public class TestQS : QuestStep
{   
    [Button("Finish Quest Step")]
    void CompleteQuestStep()
    {
        FinishQuestStep();
    }
}
