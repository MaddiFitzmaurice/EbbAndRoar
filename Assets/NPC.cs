using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private GameObject _interactUI;

    void Start()
    {
        _interactUI.gameObject.SetActive(false);
    }

    public void OnPlayerInteract(Collider other, bool isActive)
    {
        _interactUI.gameObject.SetActive(isActive);
    }
}
