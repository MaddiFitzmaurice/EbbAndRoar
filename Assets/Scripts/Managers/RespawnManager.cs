using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // Default spawn point to start of tutorial if somehow player doesn't
    // pass through a magic pool before dying
    Vector3 _currentRespawnPoint = new Vector3(-128.3f, 11.6f, 0);

    private void OnEnable()
    {
        MagicPool.NewRespawnPointEvent += NewRespawnPointEventHandler;
    }

    private void OnDisable()
    {
        MagicPool.NewRespawnPointEvent -= NewRespawnPointEventHandler;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        
        if (player.gameObject.CompareTag("Player"))
        { 
            // Reset gravity
            Physics.gravity = player.GravityNorm;
            player.gameObject.transform.position = _currentRespawnPoint;
        }
    }

    public void NewRespawnPointEventHandler(Vector3 pos)
    {
        _currentRespawnPoint = pos;
    }
}
