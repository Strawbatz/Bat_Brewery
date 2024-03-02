using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestPrerequisite : MonoBehaviour
{
    public abstract bool PrerequisiteIsMet();
}
