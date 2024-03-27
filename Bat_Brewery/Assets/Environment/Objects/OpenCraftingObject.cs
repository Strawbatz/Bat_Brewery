using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCraftingObject : InteractableObject
{
    protected override void Interact()
    {
        GameEventsManager.instance.inventoryEvents.OpenCrafting();
    }
}
