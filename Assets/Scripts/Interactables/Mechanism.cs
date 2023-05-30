using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mechanism : MonoBehaviour, Interactable
{
    [Header("Mechanism Platform Data")]
    [SerializeField] GameObject _mechanismPlatform;
    [SerializeField] Vector3 _deactivatedPosition;
    [SerializeField] Vector3 _activatedPosition;
    [SerializeField] float _timeActive;
    bool _isActive;
    
    [Header("UI")]
    [SerializeField] GameObject _interactUI;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] List<Sprite> _timerSprites;

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
        _spriteRenderer.sprite = _timerSprites[_timerSprites.Count - 1];
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponentInParent<Player>().IsLion;

        if (!isLion && !_isActive)
        {
            _interactUI.SetActive(canInteract);
            MechanismEvent?.Invoke(canInteract);
        }
    }

    void OperatedMechEventHandler()
    {
        StartCoroutine(PlatformMovement());
        StartCoroutine(TimerSpriteChange());
    }

    IEnumerator PlatformMovement()
    {
        _isActive = true;
        _interactUI.SetActive(false);
        _mechanismPlatform.transform.localPosition = _activatedPosition;
        yield return new WaitForSeconds(_timeActive);
        _mechanismPlatform.transform.localPosition = _deactivatedPosition;
        _isActive = false;
    }

    IEnumerator TimerSpriteChange()
    {
        _spriteRenderer.sprite = _timerSprites[0];
        
        foreach (Sprite sprite in _timerSprites)
        {
            yield return new WaitForSeconds(_timeActive / _timerSprites.Count);
            _spriteRenderer.sprite = sprite;
        }
    }
}
