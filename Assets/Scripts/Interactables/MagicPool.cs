using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MagicPool : MonoBehaviour, Interactable
{
    [SerializeField] GameObject _interactUI;

    // Events
    public static Action<bool> MagicPoolEvent;
    public static Action<Vector3> NewRespawnPointEvent;

    [SerializeField] Vector3 _spawnPointOffset;

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponentInParent<Player>().IsLion;

        _interactUI.SetActive(canInteract);
        
        MagicPoolEvent?.Invoke(canInteract);

        // Update current respawn point to magic pool
        NewRespawnPointEvent?.Invoke(this.transform.position + _spawnPointOffset);
    }
}
