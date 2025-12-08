using UnityEngine;
using TMPro; 
using System.Collections.Generic;

public class SimpleInventory : MonoBehaviour
{
    public static SimpleInventory instance;
    public TMP_Text[] textSlots; 

    private List<ItemData> collectedItems = new List<ItemData>();

    void Awake()
    {
        instance = this;
    }

    public void AddItem(ItemData item)
    {
        // Don't add if already in inventory otherwise go boom
        if (collectedItems.Contains(item))
        {
            return;
        }

        collectedItems.Add(item);
        UpdateUI(); 
    }

    public void RemoveItem(ItemData item)
    {
        collectedItems.Remove(item);
        UpdateUI(); 
    }

    void UpdateUI()
    {
        // Clear all slots first
        for (int i = 0; i < textSlots.Length; i++)
        {
            textSlots[i].text = "";
            textSlots[i].gameObject.SetActive(false);
        }

        // Show collected items.
        for (int i = 0; i < collectedItems.Count; i++)
        {
            textSlots[i].text = collectedItems[i].itemName; 
            textSlots[i].gameObject.SetActive(true);
        }
    }
}