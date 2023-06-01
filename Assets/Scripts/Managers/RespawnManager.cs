using Cinemachine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    // Default spawn point to start of tutorial if somehow player doesn't
    // pass through a magic pool before dying
    [SerializeField] Transform _tutorialSpawn;
    Vector3 _currentRespawnPoint;

    private void OnEnable()
    {
        MagicPool.NewRespawnPointEvent += NewRespawnPointEventHandler;
    }

    private void OnDisable()
    {
        MagicPool.NewRespawnPointEvent -= NewRespawnPointEventHandler;
    }

    void Start()
    {
        _currentRespawnPoint = _tutorialSpawn.position;
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
