using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TaggableItem : ScriptableObject
{
  public VisualTag visualTag;

  public void SetVisualTag(VisualTag newVisualTag) {
    visualTag = newVisualTag;
  }
}
