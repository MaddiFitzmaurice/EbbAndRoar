using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour, Interactable
{
    public static Action<Item> ItemPickupEvent;
    void Update()
    {
        
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        ItemPickupEvent?.Invoke(this);
        this.gameObject.SetActive(false);
    }
}
