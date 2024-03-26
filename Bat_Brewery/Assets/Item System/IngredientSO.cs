using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Scriptable Object representing ingredients.
/// </summary>
[CreateAssetMenu]
public class IngredientSO : ItemSO {
    [SerializeField] public TextAsset worldDescription;
    [TextArea(5, 10)]
    [SerializeField] public String TextbookDescription;


}
