using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicPool : MonoBehaviour, Interactable
{
    public static Action<bool> MagicPoolEvent;
    public static Action<Vector3> NewRespawnPointEvent;

    [SerializeField] Vector3 _spawnPointOffset;

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponentInParent<Player>().IsLion;

        if (!isLion)
        {
            MagicPoolEvent?.Invoke(canInteract);
            Interactable.InteractUIPromptEvent?.Invoke("Press E to transform.", canInteract);
        }
        else 
        {
            Interactable.InteractUIPromptEvent?.Invoke("The water seems unresponsive now...", canInteract);
        }

        // Update current respawn point to magic pool
        NewRespawnPointEvent?.Invoke(this.transform.position + _spawnPointOffset);
    }
}
