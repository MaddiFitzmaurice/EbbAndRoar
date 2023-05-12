using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicPool : MonoBehaviour, Interactable
{
    public static Action<bool> MagicPoolEvent;
    public void OnPlayerInteract(bool canInteract)
    {
        MagicPoolEvent?.Invoke(canInteract);
    }
}
