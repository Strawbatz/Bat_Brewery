using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextbook : MonoBehaviour
{
    public List<(string name,string desc)> playerTextbook {get; private set;}

    void Start()
    {
        playerTextbook = new List<(string,string)>();
        GameEventsManager.instance.questEvents.onTextbookDescHeard += AddTextbookDesc;
    }

    void OnDisable()
    {
        GameEventsManager.instance.questEvents.onTextbookDescHeard += AddTextbookDesc;
    }

    private void AddTextbookDesc(Ingredient ing) {
        if(playerTextbook.Contains((ing.name, ing.TextbookDescription))) {
            return;
        }
        playerTextbook.Add((ing.name, ing.TextbookDescription));
    }
}
