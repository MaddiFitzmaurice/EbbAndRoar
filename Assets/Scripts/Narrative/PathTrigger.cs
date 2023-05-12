using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//public enum PathDirection { Up = 1, Down = -1}

public class PathTrigger : MonoBehaviour
{
    public PathDirection Direction;
    public Transform ConnectedPath;

    public static Action<PathTrigger, bool> PathTriggerEvent;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PathTriggerEvent?.Invoke(this, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PathTriggerEvent?.Invoke(this, false);
        }
    }
}
