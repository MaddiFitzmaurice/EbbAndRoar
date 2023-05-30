using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PathDirection { Up = 1, Down = -1}

public class Path : MonoBehaviour, Interactable
{
    public PathDirection Direction;
    public Transform ConnectedPath;
    [SerializeField] string _UIPromptLion;
    [SerializeField] string _UIPromptHuman;
    [SerializeField] GameObject _interactUI;
    string _UIPromptText;
    public static Action<Path, bool> PathEvent;

    void Start()
    {
        _interactUI.SetActive(false);
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponentInParent<Player>().IsLion;
        
        // If is Human
        if (!isLion)
        {
            _interactUI.SetActive(canInteract);
            _UIPromptText = _UIPromptHuman;
            PathEvent?.Invoke(this, canInteract);
        }
        // If is Lion
        else 
        {
            _UIPromptText = _UIPromptLion;
        }
    }
}
