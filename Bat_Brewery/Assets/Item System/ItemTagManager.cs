using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTagManager : MonoBehaviour
{
    public static ItemTagManager instance {get; private set;}
    private VisualTag[] visualTags;
    [SerializeField] private GameObject buttonPrefab;
    private TaggableItem toBeTagged;
    [SerializeField] private GameObject buttonContainer; 

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than 1 ItemTag Manager in scene!");
        }
        instance = this; 
    }

    private void Start() {
        Debug.Log("Start");
        visualTags = Resources.LoadAll<VisualTag>("Visual Tags/Tags");
        foreach(VisualTag tag in visualTags) {
            GameObject newButton = Instantiate(buttonPrefab);
            newButton.transform.SetParent(buttonContainer.transform);
            newButton.GetComponent<TagMenuButton>().visualTag = tag;
            Debug.Log("");
        }  
    }

    public void OpenMenu(TaggableItem item) {
        
    }

    public void ItemClicked(VisualTag visualTag) {
        Debug.Log("ItemTagged");
    }
}
