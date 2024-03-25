using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Players textbook, is updated each time player 
/// learns about a new textbook description.
/// </summary>
public class PlayerTextbook : MonoBehaviour
{
    public List<(string name,string desc)> playerTextbook {get; private set;} = new List<(string,string)>();
    
    void Start()
    {
        GameEventsManager.instance.questEvents.onTextbookDescHeard += AddTextbookDesc;
    }

    void OnDisable()
    {
        GameEventsManager.instance.questEvents.onTextbookDescHeard -= AddTextbookDesc;
    }

    /// <summary>
    /// If description doesn't already exist, add it.
    /// </summary>
    /// <param name="ing">Ingredient mentioned</param>
    private void AddTextbookDesc(IngredientSO ing) {
        if(playerTextbook.Contains((ing.name, ing.TextbookDescription))) {
            return;
        }
        playerTextbook.Add((ing.name, ing.TextbookDescription));
    }
}
