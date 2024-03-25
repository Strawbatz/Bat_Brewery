using System;

public class InventoryEvents
{
    /// <summary>
    /// Is fired when an item is added to the inventory
    /// </summary>
    public event Action<ItemSO> onItemAdded;
    /// <summary>
    /// Add an item to the players inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemSO item)
    {
        if(onItemAdded != null)
        {
            onItemAdded(item);
        }
    }

    /// <summary>
    /// Is fired after an item was removed from the inventory
    /// </summary>
    public event Action<ItemSO> onItemWasRemoved;
    public void ItemWasRemoved(ItemSO item)
    {
        if(onItemWasRemoved != null)
        {
            onItemWasRemoved(item);
        }
    }

    /// <summary>
    /// Is fired when an ingredient is added to the inventory
    /// </summary>
    public event Action<IngredientSO> onPickedUpIngredient;
    /// Add an ingredient to the players inventory
    public void PickUpIngredient(IngredientSO ingredient)
    {
        if(onPickedUpIngredient != null)
        {
            onPickedUpIngredient(ingredient);
        }

        AddItem(ingredient);
    }
}
