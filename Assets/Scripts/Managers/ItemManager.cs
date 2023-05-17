using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType { Child, Tools }

public class ItemManager : MonoBehaviour
{
    [SerializeField] List<Item> _items;
    public static Action<List<Item>> UpdateItemsCollectedEvent;
    
    void OnEnable()
    {
        Item.ItemPickupEvent += UpdateItemsCollected;
        NPC.CheckItemFound += CheckItemFoundEvent;
    }

    void OnDisable()
    {
        Item.ItemPickupEvent -= UpdateItemsCollected;
        NPC.CheckItemFound -= CheckItemFoundEvent;
    }

    void UpdateItemsCollected()
    {
        UpdateItemsCollectedEvent?.Invoke(_items);
    }

    void CheckItemFoundEvent(ItemType itemNeeded)
    {
        foreach (Item item in _items)
        {
            if (item.ItemType == itemNeeded)
            {
                if (item.Found)
                {
                    item.Delivered = true;
                    UpdateItemsCollected();
                }
            }
        }
    }
}
