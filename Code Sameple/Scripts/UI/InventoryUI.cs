using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;   // The parent object of all the items
    public Sprite defaultIcon;

    Inventory inventory;    // Our current inventory
    Image[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;    // Subscribe to the onItemChanged callback

        slots = itemsParent.GetComponentsInChildren<Image>();
    }

    void UpdateUI()
    {
        // Loop through all the slots
        for (int i = 0; i < (slots.Length - 1) / 2; i++)
        {
            if (i < inventory.items.Count)  // If there is an item to add
            {
                slots[i+1].sprite = inventory.items[i].icon;
            }
            else
            {
                slots[i + 1].sprite = defaultIcon;
            }
        }

        for (int i = 0; i < (slots.Length - 1) / 2; i++)
        {
            if (i < inventory.npcs.Count)  // If there is an item to add
            {
                slots[i + 1 + (slots.Length - 1) / 2].sprite = inventory.npcs[i].icon;
            }
            else
            {
                slots[i + 1 + (slots.Length - 1) / 2].sprite = defaultIcon;
            }
        }
    }
}
