using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TagMenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button thisButton;
    [NonSerialized] public VisualTag visualTag;

    [SerializeField] private Color baseColor;
    [SerializeField] private Color selectedColor;
    // Start is called before the first frame update
    void Start()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(Clicked);
        GetComponentsInChildren<Image>()[1].sprite = visualTag.GetWorldImg();

    }

    private void Clicked() {
        ItemTagManager.instance.ItemClicked(visualTag);
    }

    public void OnSelect(BaseEventData eventData){
        GetComponentsInChildren<Image>()[0].color = selectedColor;
        GetComponentsInChildren<Image>()[2].color = new Color(1,0,0);
    }

    public void OnDeselect(BaseEventData eventData){
        GetComponentsInChildren<Image>()[0].color = baseColor;
        GetComponentsInChildren<Image>()[2].color = new Color(1,1,1);
    }

}
