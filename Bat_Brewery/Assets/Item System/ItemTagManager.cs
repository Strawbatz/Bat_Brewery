using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemTagManager : MonoBehaviour
{
    public static ItemTagManager instance {get; private set;}
    private VisualTag[] visualTags;
    private TaggableItem toBeTagged;

    [NonSerialized] public bool isOpen; 

    [SerializeField] InputActionReference leftClick;

    [Header("UI Components")]
    [SerializeField] private GameObject menuContainer;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject buttonContainer; 

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than 1 ItemTag Manager in scene!");
        }
        instance = this; 
    }

    private void Start() {
        visualTags = Resources.LoadAll<VisualTag>("Visual Tags/Tags");
        foreach(VisualTag tag in visualTags) {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(buttonContainer.transform);
            newButton.GetComponent<TagMenuButton>().visualTag = tag;
        }  
        menuContainer.SetActive(false);
    }

    public void ToggleMenu(TaggableItem item) {
        if(!menuContainer.activeSelf) {
            isOpen = true;
            menuContainer.SetActive(true);
            toBeTagged = item;
            //leftClick.action.performed += IsClickInsideContainer;
        } else {
            ExitMenu();
        }
    }

    public void ItemClicked(VisualTag visualTag) {
        Debug.Log("ItemTagged");
        toBeTagged.SetVisualTag(visualTag);
        ExitMenu();
    }

    public void ExitMenu() {
        isOpen = false;
        menuContainer.SetActive(false);
        toBeTagged = null;
        //leftClick.action.performed -= IsClickInsideContainer;
    }

    /*
    void IsClickInsideContainer(InputAction.CallbackContext ctx) {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        RectTransform container = menuContainer.GetComponent<RectTransform>();
        float rectWidth = container.rect.width;
        float rectX = (Screen.width - rectWidth)/2;
        float rectHeight =container.rect.height;
        float rectY = (Screen.height - rectHeight)/2;
        Debug.Log(rectX);
        Debug.Log(rectWidth);
        Debug.Log(rectY);
        Debug.Log(rectHeight);
        if(mousePos.x < rectX || mousePos.x > (rectX + rectWidth) || mousePos.y < rectY || mousePos.y > (rectY+rectHeight)) {
            ExitMenu();
        }
    
       // if(menuContainer.GetComponent<RectTransform>().rect.Contains(mousePos)){    ExitMenu();}
    } */
}
