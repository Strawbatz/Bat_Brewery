using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PotionCraftingController : MonoBehaviour
{    
    [SerializeField] RectTransform craftingPanel;
    [SerializeField] RectTransform inventoryPanel;
    PotionCrafting potionCrafting;
    PlayerInventory playerInventory;

    void Start()
    {
        potionCrafting = PlayerManager.instance.GetPotionCrafting();
        playerInventory = PlayerManager.instance.GetInventory();
        GameEventsManager.instance.inventoryEvents.onOpenCrafting += OpenCrafting;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void OnDisable()
    {
        GameEventsManager.instance.inventoryEvents.onOpenCrafting -= OpenCrafting;
        foreach (Transform transform in inventoryPanel)
        {
            RemoveInventorySlot(transform.gameObject);
        }
        foreach (Transform transform in craftingPanel)
        {
            RemoveInventorySlot(transform.gameObject);
        }
    }

    void OpenCrafting()
    {
        UpdateInventoryPanel();
        UpdateCraftingPanel();
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        GameEventsManager.instance.playerMovementEvents.SetFreezePlayerMovement("CraftingMenu", transform.GetChild(0).gameObject.activeSelf);
    }
    
    #region Update menu methods
    void UpdateInventoryPanel()
    {
        foreach (Transform transform in inventoryPanel)
        {
            RemoveInventorySlot(transform.gameObject);
        }
        foreach (InventoryItem ingredient in playerInventory.ingredients)
        {
            for (int i = 0; i < ingredient.count; i++)
            {
                GameObject slot = GetInventorySlot();
                slot.transform.SetParent(inventoryPanel);
                slot.GetComponentInChildren<Button>().onClick.AddListener(delegate() {AddCraftingIngredient((IngredientSO)ingredient.item);});
                slot.GetComponentInChildren<Image>().sprite = ItemTagManager.instance.GetVisualTag((IngredientSO)ingredient.item).GetInventoryImg();
                slot.SetActive(true);
            }
        }
    }

    void UpdateCraftingPanel()
    {
        foreach (Transform transform in craftingPanel)
        {
            RemoveInventorySlot(transform.gameObject);
        }
        foreach (IngredientSO ingredient in potionCrafting.addedIngredients)
        {

            GameObject slot = GetInventorySlot();
            slot.transform.SetParent(craftingPanel);
            slot.GetComponentInChildren<Button>().onClick.AddListener(delegate() {RemoveCraftingIngredient(ingredient);});
            slot.GetComponentInChildren<Image>().sprite = ItemTagManager.instance.GetVisualTag(ingredient).GetInventoryImg();
            slot.SetActive(true);
        }
    }
    #endregion

    #region Crafting methods
    void AddCraftingIngredient(IngredientSO ingredient)
    {
        if(!potionCrafting.HasAvailableSlot()) return;
        if(playerInventory.RemoveItem(ingredient))
        {
            potionCrafting.addedIngredients.Add(ingredient);
        }

        UpdateCraftingPanel();
        UpdateInventoryPanel();
    }
    void RemoveCraftingIngredient(IngredientSO ingredient)
    {
        if(!playerInventory.HasAvailableIngredientSlot()) return;
        if(potionCrafting.addedIngredients.Contains(ingredient))
        {
            potionCrafting.addedIngredients.Remove(ingredient);
            GameEventsManager.instance.inventoryEvents.AddItem(ingredient);
        }
        UpdateCraftingPanel();
        UpdateInventoryPanel();
    }

    /// <summary>
    /// Crafts the potion using the currently added ingredients.
    /// </summary>
    public void Craft()
    {
        PotionSO potion = potionCrafting.CraftPotion();
        if(potion != null)
        {
            QuestItemSO item = potion;
            GameEventsManager.instance.inventoryEvents.AddItem(item);
        }
        UpdateCraftingPanel();
        UpdateInventoryPanel();
    }
    #endregion

    #region Object pooling
    [SerializeField] GameObject inventorySlotPrefab;
    Stack<GameObject> inventorySlotPol = new Stack<GameObject>();
    private GameObject GetInventorySlot()
    {
        if(inventorySlotPol.Count > 0)
        {
            return inventorySlotPol.Pop();
        } 

        return GameObject.Instantiate(inventorySlotPrefab);
    }

    private void RemoveInventorySlot(GameObject inventorySlot)
    {
        inventorySlot.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        inventorySlot.SetActive(false);
        inventorySlotPol.Push(inventorySlot);
    }
    #endregion
}
