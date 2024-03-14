using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using Unity.VisualScripting;
using UnityEngine;

public class TextbookController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject inactiveContainer;
    [SerializeField] private GameObject tbHeaderPrefab;
    [SerializeField] private GameObject tbDescription;

    private List<RectTransform> tbItems = new List<RectTransform>();
    private List<(string name, string desc)> playerTB;

    private int? descPosition;

    private void Start() {
        GameEventsManager.instance.inputEvents.onTabInteracted += ToggleTextbook;
        panel.SetActive(false);
        playerTB = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTextbook>().playerTextbook;
        int i = 0;
        foreach(RectTransform item in container.GetComponentInChildren<RectTransform>()) {
            tbItems.Add(item);
            item.GetComponent<TextbookItem>().nr = i;
            i++;
        }
        PositionHeaders(0);
    }

    private void OnDestroy() {
        GameEventsManager.instance.inputEvents.onTabInteracted -= ToggleTextbook;
    }

    private void ToggleTextbook() {
        panel.SetActive(!panel.activeSelf);
        if(panel.activeSelf) {
            OpenTextbook();
        } else if(!panel.activeSelf) {
            CloseTextbook();
        }
    }

    private void OpenTextbook() {
        if(tbItems.Count < playerTB.Count) {
            AddTBHeaders(playerTB);
            PositionHeaders(0);
        } else if( tbItems.Count > 0) {
            PositionHeaders(0);
        }
    }

    private void CloseTextbook() {
        if(descPosition != null) {
            RectTransform descRect = tbDescription.GetComponent<RectTransform>();
            descRect.SetParent(inactiveContainer.transform);
            descPosition = null;
        }
    }

    private void AddTBHeaders(List<(string name,string desc)> playerTB) {
        for(int i=tbItems.Count; i<playerTB.Count; i++) {
            RectTransform item = Instantiate(tbHeaderPrefab).GetComponentInChildren<RectTransform>();
            item.SetParent(container.transform);
            tbItems.Add(item);
            item.GetComponent<TextbookItem>().nr = i;
        }
    }

    private void PositionHeaders(int start) {
        if(start > tbItems.Count) {Debug.Log("trying to position tbItems outside bounds"); return;}
        for(int i=start; i<tbItems.Count; i++){
            if(i == 0) {
                tbItems[i].transform.localPosition = new Vector3(0, -2, 0);
                continue;
            }
            tbItems[i].transform.localPosition = new Vector3(0, tbItems[i-1].transform.localPosition.y-tbItems[i-1].rect.height, 0);
        }
    }

    public void ItemClicked(int pos) {
        RectTransform descRect = tbDescription.GetComponent<RectTransform>();
        if(descPosition != null){
            if (pos == descPosition-1) {
                tbItems.RemoveAt((int)descPosition);
                descRect.SetParent(inactiveContainer.transform);
                descPosition = null;
                PositionHeaders(0);
                return;
            }
            tbItems.RemoveAt((int)descPosition);
            PositionHeaders(0);
        }
        tbItems.Insert(pos+1, descRect);
        descPosition = pos+1;
        descRect.SetParent(container.transform);
        PositionHeaders(pos+1);
    }
}
