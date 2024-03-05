using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTagManager : MonoBehaviour
{
    public static ItemTagManager instance {get; private set;}
    private VisualTag[] visualTags;
    private TaggableItem toBeTagged;

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

    public void OpenMenu(TaggableItem item) {
        menuContainer.SetActive(true);
        toBeTagged = item;
    }

    public void ItemClicked(VisualTag visualTag) {
        Debug.Log("ItemTagged");
        toBeTagged.SetVisualTag(visualTag);
        ExitMenu();
    }

    private void ExitMenu() {
        menuContainer.SetActive(false);
        toBeTagged = null;
    }
}
