using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestPrerequisite : MonoBehaviour
{
    /// <summary>
    /// Returns true if the prerequisite is met
    /// </summary>
    /// <returns></returns>
    public abstract bool PrerequisiteIsMet();
}
