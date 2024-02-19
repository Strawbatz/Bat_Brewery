using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
/// <summary>
/// Class for taggable Items
/// </summary>
public class TaggableItem : ScriptableObject
{
    //Visual tag currently used.
  public VisualTag visualTag;

  //Lets taggableItems know that their visual tag is updated.
  public UnityAction tagUpdated;

  public void SetVisualTag(VisualTag newVisualTag) {
    visualTag = newVisualTag;
    tagUpdated?.Invoke();
  }
}
