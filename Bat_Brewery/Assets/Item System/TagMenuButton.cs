using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Script for button functionallity in tagMenu.
/// </summary>
public class TagMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button thisButton;
    [NonSerialized] public VisualTagSO visualTag;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color selectedColor;
    private bool lastSelected = false;
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(Clicked);
        GetComponentsInChildren<Image>()[1].sprite = visualTag.GetWorldImg();
        transform.localScale = Vector3.one;
    }

    public void SetLastSelected() {
        lastSelected = true;
    }

    private void Clicked() {
        ItemTagManager.instance.ItemClicked(visualTag);
    }

    public void OnSelect(BaseEventData eventData){
        GetComponentsInChildren<Image>()[0].color = selectedColor;
        GetComponentsInChildren<Image>()[2].color = new Color(1,0,0);
    }

    public void OnDeselect(BaseEventData eventData){
        if(lastSelected) {
            GetComponentsInChildren<Image>()[0].color = new Color(0.5f,0.5f,0.5f);
            GetComponentsInChildren<Image>()[2].color = new Color(0.5f,0.5f,0.5f);
        } else {
            GetComponentsInChildren<Image>()[0].color = baseColor;
            GetComponentsInChildren<Image>()[2].color = new Color(1,1,1);
        }
    }

    private void OnDisable() {
        GetComponentsInChildren<Image>()[0].color = baseColor;
        GetComponentsInChildren<Image>()[2].color = new Color(1,1,1);
        lastSelected = false;
    }

}
