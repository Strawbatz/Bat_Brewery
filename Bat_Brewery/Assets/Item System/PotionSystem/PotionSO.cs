using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents the potions
/// </summary>
[CreateAssetMenu]
public class PotionSO : QuestItemSO
{
    [SerializeField] IngredientSO[] ingredients;

    public IngredientSO[] GetIngredients()
    {
        return ingredients;
    }
}
