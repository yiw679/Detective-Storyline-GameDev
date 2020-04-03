using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    void Awake()
    {
        if (instance != null)
            Debug.Log("More Than One");
        instance = this;
    }

    // Callback which is triggered when
    // an item gets added/removed.
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 7;  // Amount of slots in inventory

    // Current list of items in inventory
    public List<HI_Prop> items = new List<HI_Prop>();
    public List<EnemyAI> npcs = new List<EnemyAI>();
    private Button[] btns;

    private void Start()
    {
        btns = GetComponentsInChildren<Button>();

        int i;
        for(i = 0; i < space; i++)
        {
            addItemListener(btns[i],i);
        }
        for(i = space; i < space * 2; i++)
        {
            addNPCListener(btns[i], i - space);
        }
    }

    public static void addItemListener(Button leButton, int idx)
    {
        leButton.onClick.AddListener(() => ShowDetail(idx));
    }

    public static void ShowDetail(int idx)
    {
        if (idx < Inventory.instance.items.Count && Inventory.instance.items[idx])
        {
            Inventory.instance.items[idx].ShowDetail(idx);
        }
    }

    public static void addNPCListener(Button leButton, int idx)
    {
        leButton.onClick.AddListener(() => ShowIntro(idx));
    }

    public static void ShowIntro(int idx)
    {
        if (idx< Inventory.instance.npcs.Count && Inventory.instance.npcs[idx])
        {
            Inventory.instance.npcs[idx].ShowIntro();
        }
    }

    // Add a new item. If there is enough room we
    // return true. Else we return false.
    public bool Add(HI_Prop item)
    {
        // Check if out of space
        if (items.Count >= space)
        {
            Debug.Log("Inventory Full");
            return false;
        }

        items.Add(item);    // Add item to list

        // Trigger callback
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    public bool Add(EnemyAI npc)
    {
        // Check if out of space
        if (npcs.Count >= space)
        {
            Debug.Log("Inventory Full");
            return false;
        }

        npcs.Add(npc);    // Add item to list

        // Trigger callback
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        return true;
    }

    // Remove an item
    public void Remove(HI_Prop item)
    {
        items.Remove(item);     // Remove item from list

        // Trigger callback
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }
}
