using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Child : MonoBehaviour, Interactable
{
    NPCEventData _npcEventData;
    // NPC Data
    [Header("Child Data")]
    [SerializeField] Color _colour;
    [SerializeField] Transform _startPos;
    [SerializeField] Transform _hidingPos;
    [SerializeField] Transform _endPos;

    // Interaction
    [Header("UI Interact Prompt")]
    [SerializeField] private GameObject _greetingUI;
    [SerializeField] private GameObject _questUI;
    [SerializeField] private GameObject _interactUI;
    private TextMeshProUGUI _greetingText;

    // Flavour Text
    [Header("Greeting Lion Dialogue")]
    [SerializeField] string _greetingLionText;

    [Header("Quest Dialogue")]
    [SerializeField] TextAsset _childText;
    bool _isTalking;

    // Components
    SpriteRenderer _spriteRenderer;

    // Event
    public static Action ChildEvent;

    void OnEnable()
    {
        PlayerNarrativeState.StartNarrativeEvent += HideGreeting;
        NarrativeManager.EndOfNarrativeEvent += OnPlayerDialogueFinished;
        ItemManager.ChildReturnedHomeEvent += ReturnHome;
    }

    void OnDisable()
    {
        PlayerNarrativeState.StartNarrativeEvent -= HideGreeting;
        NarrativeManager.EndOfNarrativeEvent -= OnPlayerDialogueFinished;
        ItemManager.ChildReturnedHomeEvent -= ReturnHome;
    }

    void Start()
    {        
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sortingLayerName = "Z0";
        UISetup();
        transform.position = _startPos.position;
        _npcEventData = new NPCEventData(false, false, _startPos, _childText, _colour);
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponentInParent<Player>().IsLion;

        // Find out what side of the player the NPC is on
        _npcEventData.IsOnRightSide = player.transform.position.x > transform.position.x ? false : true;
        
        // If player is not a lion
        if (!isLion)
        {
            _interactUI.SetActive(canInteract);
            _npcEventData.CanInteract = canInteract;
            NPC.SendNarrativeDataEvent?.Invoke(_npcEventData);
        }
        // If player is a lion
        else
        {
            _greetingUI.SetActive(canInteract);
        }

        _isTalking = canInteract;
        DisplayGreeting(canInteract);
    }

    void UISetup()
    {
        _greetingUI.gameObject.SetActive(false);
        _questUI.gameObject.SetActive(true);
        _greetingText = _greetingUI.GetComponentInChildren<TextMeshProUGUI>();
        _greetingText.text = _greetingLionText;
    }

    // Display quest marker
    void DisplayGreeting(bool displayGreeting)
    {        
        _questUI.gameObject.SetActive(!displayGreeting);
    }

    void HideGreeting(bool displayGreeting)
    {
        if (_npcEventData.CanInteract && displayGreeting)
        {
            _interactUI.SetActive(!displayGreeting);
            _greetingUI.gameObject.SetActive(!displayGreeting);
            _questUI.gameObject.SetActive(!displayGreeting);
        }
    }

    void OnPlayerDialogueFinished()
    {
        if (_isTalking)
        {
            // Signals child dialogue has been completed
            if (_npcEventData.CurrentDialogue == _childText)
            {
                ChildEvent?.Invoke();
                transform.position = _hidingPos.position;
            }
        }
       
       _isTalking = false;
    }

    void ReturnHome()
    {
        transform.position = _endPos.position;
        _spriteRenderer.sortingLayerName = "Z3";
        _questUI.SetActive(false);
        _greetingUI.SetActive(false);
        _interactUI.SetActive(false);
        this.gameObject.SetActive(true);
    }
}
