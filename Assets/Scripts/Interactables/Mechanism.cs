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
    [SerializeField] Vector3 _deactivatedPosition;
    [SerializeField] Vector3 _activatedPosition;
    [SerializeField] float _timeActive;

    public static Action<bool> MechanismEvent;

    void OnEnable()
    {
        HumanMoveState.OperatedMechEvent += OperatedMechEventHandler;
    }

    void OnDisable()
    {
        HumanMoveState.OperatedMechEvent -= OperatedMechEventHandler;
    }

    void Start()
    {
        _mechanismPlatform.transform.localPosition = _deactivatedPosition;
    }

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

    void OperatedMechEventHandler()
    {
        StartCoroutine(PlatformMovement());
    }

    IEnumerator PlatformMovement()
    {
        Debug.Log("Huh");
        _mechanismPlatform.transform.localPosition = _activatedPosition;
        yield return new WaitForSeconds(_timeActive);
        _mechanismPlatform.transform.localPosition = _deactivatedPosition;
    }
}
