using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    private NPC _npc;

    void Awake()
    {
        _npc = GetComponentInParent<NPC>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _npc.OnPlayerInteract(other, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _npc.OnPlayerInteract(other, false);
        }
    }
}
