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
    public static Action ChildReturnedHomeEvent;
    public static Action AllItemsDeliveredEvent;
    
    void OnEnable()
    {
        Item.ItemPickupEvent += UpdateItemsFound;
        NPC.CheckItemFound += CheckItemFoundEvent;
        NPC.ItemDeliveredEvent += UpdateItemsDelivered;
        Child.ChildEvent += ChildEventHandler;
    }

    void OnDisable()
    {
        Item.ItemPickupEvent -= UpdateItemsFound;
        NPC.CheckItemFound -= CheckItemFoundEvent;
        NPC.ItemDeliveredEvent -= UpdateItemsDelivered;
        Child.ChildEvent -= ChildEventHandler;
    }

    void UpdateItemsFound()
    {
        UpdateItemsFoundEvent?.Invoke(_items);
    }

    void UpdateItemsDelivered(ItemType itemTypeFound)
    {
        // Child returns home
        if (itemTypeFound == ItemType.Child)
        {
            ChildReturnedHomeEvent?.Invoke();
        }
        
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

        CheckAllItemsDelivered();
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

    void CheckAllItemsDelivered()
    {
        foreach (Item item in _items)
        {
            if (item.Delivered == false)
            {
                return;
            }
        }

        AllItemsDeliveredEvent?.Invoke();
    }

    // Activate child as an item instead of an NPC
    void ChildEventHandler()
    {
        foreach (Item item in _items)
        {
            if (item.ItemType == ItemType.Child)
            {
                item.gameObject.SetActive(true);
            }
        }
    }
}
