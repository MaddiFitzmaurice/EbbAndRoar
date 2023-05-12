using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicPool : MonoBehaviour, Interactable
{
    public static Action<bool> MagicPoolEvent;
    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponent<Player>().IsLion;

        if (!isLion)
        {
            MagicPoolEvent?.Invoke(canInteract);
            Interactable.InteractUIPromptEvent?.Invoke("Press E to transform.", canInteract);
        }
        else 
        {
            Interactable.InteractUIPromptEvent?.Invoke("The pool seems unresponsive now...", canInteract);
        }
    }
}
