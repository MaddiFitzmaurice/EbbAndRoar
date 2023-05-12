using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PathDirection { Up = 1, Down = -1}

public class Path : MonoBehaviour, Interactable
{
    public PathDirection Direction;
    public Transform ConnectedPath;

    public static Action<Path, bool> PathEvent;

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        PathEvent?.Invoke(this, canInteract);
    }
}
