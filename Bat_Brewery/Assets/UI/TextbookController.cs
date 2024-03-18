using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Controller for the textbook UI.
/// </summary>
public class TextbookController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject inactiveContainer;
    [SerializeField] private GameObject tbHeaderPrefab;
    [SerializeField] private GameObject tbDescription;

    [SerializeField] private Sprite rightArrow;
    [SerializeField] private Sprite downArrow;

    private List<RectTransform> tbItems = new List<RectTransform>();
    private List<(string name, string desc)> playerTB;

    private int? descPosition;

    private void Start() {
        GameEventsManager.instance.inputEvents.onTabInteracted += ToggleTextbook;
        panel.SetActive(false);
        playerTB = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTextbook>().playerTextbook;

        PositionHeaders(0);
    }

    private void OnDestroy() {
        GameEventsManager.instance.inputEvents.onTabInteracted -= ToggleTextbook;
    }

    /// <summary>
    /// Toggles textbook UI and calls appropiate methods.
    /// </summary>
    private void ToggleTextbook() {
        panel.SetActive(!panel.activeSelf);
        if(panel.activeSelf) {
            OpenTextbook();
        } else if(!panel.activeSelf) {
            CloseTextbook();
        }
    }

    /// <summary>
    /// When opening textbook, checks if any new igredients have been learnt
    /// and if so adds them, then asks to position all items.
    /// </summary>
    private void OpenTextbook() {
        if(tbItems.Count < playerTB.Count) {
            AddTBHeaders(playerTB);
            PositionHeaders(0);
        } else if( tbItems.Count > 0) {
            PositionHeaders(0);
        }
    }

    /// <summary>
    /// When closing textbook set pointers and similar to correct values for 
    /// next use. 
    /// </summary>
    private void CloseTextbook() {
        if(descPosition != null) {
            RectTransform descRect = tbDescription.GetComponent<RectTransform>();
            descRect.SetParent(inactiveContainer.transform);
            tbItems.RemoveAt((int)descPosition);
            Image arrow = tbItems[(int)descPosition-1].GetComponentsInChildren<Image>()[2];
            descPosition = null;
            arrow.sprite = rightArrow;
        }
    }

    /// <summary>
    /// Adds UI components for each textbook description the player knows
    /// but is not in UI yet and sets it upp.
    /// </summary>
    /// <param name="playerTB"></param>
    private void AddTBHeaders(List<(string name,string desc)> playerTB) {
        for(int i=tbItems.Count; i<playerTB.Count; i++) {
            RectTransform item = Instantiate(tbHeaderPrefab).GetComponentInChildren<RectTransform>();
            item.SetParent(container.transform);
            tbItems.Add(item);
            item.GetComponent<TextbookItem>().nr = i;

            if(playerTB[i].desc != null) { //Populate Names
                item.GetComponentInChildren<TextMeshProUGUI>().text = playerTB[i].name;
            }
        }
    }

    /// <summary>
    /// Positions each UI element in list after last elements position. 
    /// </summary>
    /// <param name="start">From which position to start updating from</param>
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

    /// <summary>
    /// When a textbook description header is clicked, handles functionallity depending
    /// on what currently is active. This includes assigning description to the right place, 
    /// populating it and changing arrow indicator. 
    /// </summary>
    /// <param name="pos">Position of clicked item</param>
    public void ItemClicked(int pos) {
        RectTransform descRect = tbDescription.GetComponent<RectTransform>();
        if(descPosition != null){ //if an item was previously clicked
            if (pos == descPosition-1) { //if item clicked was the same as currently open
                tbItems.RemoveAt((int)descPosition);
                descRect.SetParent(inactiveContainer.transform);
                descPosition = null;
                tbItems[pos].GetComponentsInChildren<Image>()[2].sprite = rightArrow;
                PositionHeaders(0);
                EventSystem.current.SetSelectedGameObject(null);
                return;
            }
            tbItems.RemoveAt((int)descPosition);
            tbItems[(int)descPosition-1].GetComponentsInChildren<Image>()[2].sprite = rightArrow;
            PositionHeaders(0);
        } //Open new description
        tbItems.Insert(pos+1, descRect);
        descPosition = pos+1;
        descRect.SetParent(container.transform);
        tbItems[pos].GetComponentsInChildren<Image>()[2].sprite = downArrow;
        
        //Populate description
        descRect.GetComponentInChildren<TextMeshProUGUI>().text = playerTB[pos].desc;

        PositionHeaders(pos+1);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
