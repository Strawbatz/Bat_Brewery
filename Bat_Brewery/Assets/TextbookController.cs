using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using Unity.VisualScripting;
using UnityEngine;

public class TextbookController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject inactiveContainer;
    [SerializeField] private GameObject tbHeaderPrefab;
    [SerializeField] private GameObject tbDescription;

    [SerializeField] private List<RectTransform> tbItems = new List<RectTransform>();

    private void Start() {
        int i = 0;
        foreach(RectTransform item in container.GetComponentInChildren<RectTransform>()) {
            tbItems.Add(item);
            item.GetComponent<TextbookItem>().nr = i;
            i++;
        }
        PositionHeaders(0);
    }

    private void PositionHeaders(int start) {
        for(int i=start; i<tbItems.Count; i++){
            if(i == 0) {
                tbItems[i].transform.localPosition = new Vector3(0, -2, 0);
                continue;
            }
            tbItems[i].transform.localPosition = new Vector3(0, tbItems[i-1].transform.localPosition.y-tbItems[i-1].rect.height, 0);
        }
    }

    public void ItemClicked(int pos) {
        
    }
}
