using UnityEngine;

public class Item : Interactable
{
    [Header("Settings")]
    
    public ItemData itemData; 

    public override void Interact()
    {
        base.Interact(); 

        if (itemData != null)
        {
            if (SimpleInventory.instance != null)
            {
                SimpleInventory.instance.AddItem(itemData);
            }

            gameObject.SetActive(false);
        }
    }
}