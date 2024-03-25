using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IngredientSO : ItemSO {
    [SerializeField] public TextAsset worldDescription;
    [TextArea(5, 10)]
    [SerializeField] public String TextbookDescription;


}
