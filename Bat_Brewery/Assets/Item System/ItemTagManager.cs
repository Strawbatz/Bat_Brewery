using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            StartCoroutine(SelectFirstChoice(Array.IndexOf(visualTags, item.visualTag)));
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

    /// <summary>
    /// Corutine for selecting a first choice for navigaition
    /// with controller/keyboard. Waiting a frame between deselecting
    /// and selecting a new item for the eventsystem is needed because 
    /// unity is unity.
    /// </summary>
    ///  <param name="nr">Index of item to be selected.</param>
    /// <returns></returns>
    private IEnumerator SelectFirstChoice(int nr){
        Debug.Log(nr);
        if(nr <= -1 || nr > visualTags.Length) {
            nr = 0;
        } else {
            buttonContainer.GetComponentsInChildren<TagMenuButton>()[nr].SetLastSelected();
        }
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(buttonContainer.GetComponentsInChildren<Button>()[nr].gameObject);
        
    }
}
