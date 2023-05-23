using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType { Child, Tools, Weapons }

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    public static Action<List<Item>> UpdateItemsFoundEvent;
    public static Action UpdateItemsDeliveredEvent;
    
    void OnEnable()
    {
        Item.ItemPickupEvent += UpdateItemsFound;
        NPC.CheckItemFound += CheckItemFoundEvent;
        NPC.ItemDeliveredEvent += UpdateItemsDelivered;
    }

    void OnDisable()
    {
        Item.ItemPickupEvent -= UpdateItemsFound;
        NPC.CheckItemFound -= CheckItemFoundEvent;
        NPC.ItemDeliveredEvent -= UpdateItemsDelivered;
    }

    void UpdateItemsFound()
    {
        UpdateItemsFoundEvent?.Invoke(_items);
    }

    void UpdateItemsDelivered(ItemType itemTypeFound)
    {
        foreach (Item item in _items)
        {
            if (item.ItemType == itemTypeFound)
            {
                if (item.Found)
                {
                    item.Delivered = true;
                    UpdateItemsDeliveredEvent?.Invoke();
                }
            }
        }
    }

    bool CheckItemFoundEvent(ItemType itemNeeded)
    {
        foreach (Item item in _items)
        {
            if (item.ItemType == itemNeeded)
            {
                if (item.Found)
                {
                    UpdateItemsFound();
                    return true;
                }
            }
        }

        return false;
    }
}
