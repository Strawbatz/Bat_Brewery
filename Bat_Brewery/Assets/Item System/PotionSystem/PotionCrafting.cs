using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the potion crafting in the game
/// </summary>
public class PotionCrafting : MonoBehaviour
{
    [SerializeField] int maxCraftingSlots;
    [SerializeField] PotionSO failedPotion;
    /// <summary>
    /// The ingredients added to the potion crafting
    /// </summary>
    public List<IngredientSO> addedIngredients = new List<IngredientSO>();
    PotionSO[] potions = new PotionSO[0];

    private void Start() 
    {
        potions = Resources.LoadAll<PotionSO>("Potions");
    }

    /// <summary>
    /// Try to craft a potion with the ingredients added to the potion crafting. The ingredients added to the crafting will also be cleared.
    /// </summary>
    /// <param name="recipePotion"></param>
    /// <returns></returns>
    public PotionSO CraftPotion()
    {   
        if(addedIngredients.Count <= 0) return null;
        foreach (PotionSO recipePotion in potions)
        {
            bool ok = true;
            if(recipePotion.GetIngredients().Length != addedIngredients.Count) break;
            foreach(IngredientSO ingredient in recipePotion.GetIngredients())
            {
                if(!addedIngredients.Contains(ingredient))
                {
                    ok = false;
                    break;   
                }
            }
            if(!ok) break;
            
            addedIngredients = new List<IngredientSO>();
            return recipePotion;
        }
        addedIngredients = new List<IngredientSO>();
        return failedPotion;
    }

    /// <summary>
    /// Add crafting slots
    /// </summary>
    /// <param name="slots"></param>
    public void AddCraftingSlots(int slots)
    {
        maxCraftingSlots += slots;
    }

    /// <summary>
    /// Returns how many crafting slots that are available
    /// </summary>
    /// <returns></returns>
    public int GetCraftingSlots()
    {
        return maxCraftingSlots;
    }
    
    /// <summary>
    /// Returns true if the potion crafting system has an available slot for crafting
    /// </summary>
    /// <returns></returns>
    public bool HasAvailableSlot()
    {
        return addedIngredients.Count < maxCraftingSlots;
    }

}
