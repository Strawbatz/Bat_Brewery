using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Singleton manager for handling the tagMenu system.
/// </summary>
public class ItemTagManager : MonoBehaviour
{
    public static ItemTagManager instance {get; private set;}
    private VisualTag[] visualTags;
    private TaggableItem toBeTagged;

    [NonSerialized] public bool isOpen; 

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

    /// <summary>
    /// Function for toggling the menu on and off. Is only toggleable
    /// given an item to change tags on.
    /// </summary>
    /// <param name="item">taggable item to be changed.</param>
    public void ToggleMenu(TaggableItem item) {
        if(!menuContainer.activeSelf) {
            isOpen = true;
            menuContainer.SetActive(true);
            toBeTagged = item;
        } else {
            ExitMenu();
        }
    }

    /// <summary>
    /// Function called by the buttons inside the menu, providing
    /// the visual tag to be set for the item.
    /// </summary>
    /// <param name="visualTag">Visual tag to be set.</param>
    public void ItemClicked(VisualTag visualTag) {
        toBeTagged.SetVisualTag(visualTag);
        ExitMenu();
    }

    /// <summary>
    /// Closes menu weither an item was selected or not.
    /// </summary>
    public void ExitMenu() {
        isOpen = false;
        menuContainer.SetActive(false);
        toBeTagged = null;
    }
}