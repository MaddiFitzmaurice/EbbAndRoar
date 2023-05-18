using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item : MonoBehaviour, Interactable
{
    public bool Found;
    public bool Delivered;
    public ItemType ItemType; 
    public Sprite Sprite;
    public static Action ItemPickupEvent;

    [SerializeField] string _UIPromptLion;
    [SerializeField] string _UIPromptHuman;
    string _UIPromptText;

    void Start()
    {
        Found = false;
        Delivered = false;
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        // Potentially restrict player to only pickup as human
        Found = true;
        ItemPickupEvent?.Invoke();
        this.gameObject.SetActive(false);
    }
}
