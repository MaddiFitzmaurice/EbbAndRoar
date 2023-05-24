using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mechanism : MonoBehaviour, Interactable
{
    [SerializeField] string _UIPromptHuman;
    [SerializeField] string _UIPromptLion;
    string _UIPrompt;

    [SerializeField] GameObject _mechanismPlatform;
    [SerializeField] float _deactivatedPosition;
    [SerializeField] float _activatedPosition;
    [SerializeField] float _timeActive;

    public static Action<bool> MechanismEvent;

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponent<Player>().IsLion;

        if (!isLion)
        {
            MechanismEvent?.Invoke(canInteract);
            _UIPrompt = _UIPromptHuman;
        }
        else 
        {
            _UIPrompt = _UIPromptLion;
        }

        Interactable.InteractUIPromptEvent?.Invoke(_UIPrompt, canInteract);
    }
}
