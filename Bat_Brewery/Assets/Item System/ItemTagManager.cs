using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Singleton manager for handling the tagMenu system.
/// </summary>
public class ItemTagManager : MonoBehaviour
{
    [SerializeField] VisualTagSO defaultVisualTag;
    public static ItemTagManager instance {get; private set;}
    private VisualTagSO[] visualTags;
    private IngredientSO toBeTagged;
    Dictionary<IngredientSO, VisualTagSO>  ingredientTagDictionary = new Dictionary<IngredientSO, VisualTagSO>();
    public UnityAction<IngredientSO> onVisualTagUpdated;

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

        //TODO LOAD ALL VISUAL TAGS FROM SAVE FILE
    }

    private void Start() {
        visualTags = Resources.LoadAll<VisualTagSO>("Visual Tags/Tags");
        foreach(VisualTagSO tag in visualTags) {
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
    /// <param name="ingredient">taggable item to be changed.</param>
    public void ToggleMenu(IngredientSO ingredient) {
        if(!menuContainer.activeSelf) {
            GameEventsManager.instance.playerMovementEvents.SetFreezePlayerMovement("tagMenu", true);
            isOpen = true;
            menuContainer.SetActive(true);
            toBeTagged = ingredient;
            StartCoroutine(SelectFirstChoice(Array.IndexOf(visualTags, GetVisualTag(ingredient))));
        } else {
            ExitMenu();
        }
    }

    /// <summary>
    /// Function called by the buttons inside the menu, providing
    /// the visual tag to be set for the item.
    /// </summary>
    /// <param name="visualTag">Visual tag to be set.</param>
    public void ItemClicked(VisualTagSO visualTag) {
        SetVisualTag(toBeTagged, visualTag);
        ExitMenu();
    }

    /// <summary>
    /// Closes menu weither an item was selected or not.
    /// </summary>
    public void ExitMenu() {
        isOpen = false;
        menuContainer.SetActive(false);
        toBeTagged = null;
        GameEventsManager.instance.playerMovementEvents.SetFreezePlayerMovement("tagMenu", false);
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

    /// <summary>
    /// Set the visual tag of the ingredient
    /// </summary>
    /// <param name="ingredientSO"></param>
    /// <param name="visualTagSO"></param>
    private void SetVisualTag(IngredientSO ingredientSO, VisualTagSO visualTagSO)
    {
        if(ingredientTagDictionary.ContainsKey(ingredientSO))
        {
            ingredientTagDictionary[ingredientSO] = visualTagSO;
        } else
        {
            ingredientTagDictionary.Add(ingredientSO, visualTagSO);
        }

        onVisualTagUpdated?.Invoke(ingredientSO);
    }

    /// <summary>
    /// Get the visual tag of the ingredient
    /// </summary>
    /// <param name="ingredientSO"></param>
    /// <returns></returns>
    public VisualTagSO GetVisualTag(IngredientSO ingredientSO)
    {
        if(ingredientTagDictionary.ContainsKey(ingredientSO))
        {
            return ingredientTagDictionary[ingredientSO];
        }
        return defaultVisualTag;
    }
}
