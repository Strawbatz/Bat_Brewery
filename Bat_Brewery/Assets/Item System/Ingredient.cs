using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles Ingredients in runtime
/// </summary>
public class Ingredient
{
    public IngredientSO ingredientSO {get; private set;}
    //Visual tag currently used.
    public VisualTagSO visualTag {get; private set;}

    //Lets taggableItems know that their visual tag is updated.
    public UnityAction tagUpdated;

    public Ingredient(IngredientSO ingredientSO, VisualTagSO visualTagSO)
    {
        this.ingredientSO = ingredientSO;
    }

    /// <summary>
    /// Set the visual tag of this object
    /// </summary>
    /// <param name="newVisualTag"></param>
    public void SetVisualTag(VisualTagSO newVisualTag) 
    {
        visualTag = newVisualTag;
        tagUpdated?.Invoke();
    }
}
