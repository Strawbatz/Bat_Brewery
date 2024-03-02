using System;

public class InventoryEvents
{
    /// <summary>
    /// Is fired when an ingredient is added to the inventory
    /// </summary>
    public event Action<Ingredient> onPickUpIngredient;
    /// <summary>
    /// Add an ingredient to the players inventory
    /// </summary>
    /// <param name="ingredient"></param>
    public void PickUpIngredient(Ingredient ingredient)
    {
        if(onPickUpIngredient != null)
        {
            onPickUpIngredient(ingredient);
        }
    }

    /// <summary>
    /// Is fired after an ingredient was removed from the inventory
    /// </summary>
    public event Action<Ingredient> onIngredientWasRemoved;
    public void IngredientWasRemoved(Ingredient ingredient)
    {
        if(onIngredientWasRemoved != null)
        {
            onIngredientWasRemoved(ingredient);
        }
    }
}
