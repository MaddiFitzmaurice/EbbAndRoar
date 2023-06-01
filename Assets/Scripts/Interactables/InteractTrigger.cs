using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    private Interactable _interactable;

    void Awake()
    {
        _interactable = GetComponentInParent<Interactable>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("WTF");
            _interactable.OnPlayerInteract(other, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OOOO");
            _interactable.OnPlayerInteract(other, false);
        }
    }
}
