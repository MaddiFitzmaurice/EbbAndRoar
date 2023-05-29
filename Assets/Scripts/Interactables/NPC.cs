using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public struct NPCEventData
{
    public bool CanInteract;
    public bool IsOnRightSide;
    public Transform Transform;
    public TextAsset CurrentDialogue;
    public Color Colour;

    public NPCEventData(bool canInteract, bool isOnRightSide, Transform transform, TextAsset currentDialogue, Color colour)
    {
        CanInteract = canInteract;
        IsOnRightSide = isOnRightSide;
        Transform = transform;
        CurrentDialogue = currentDialogue;
        Colour = colour;
    }
}

public class NPC : MonoBehaviour, Interactable
{
    // NPC Data
    [Header("NPC Data")]
    [SerializeField] Color _colour;

    // Interaction
    [Header("UI Interact Prompt")]
    [SerializeField] private GameObject _interactUI;
    private TextMeshProUGUI _interactText;
    [SerializeField] string _UIPromptLion;
    [SerializeField] string _UIPromptHuman;
    string _UIPromptText;
    
    // Billboarding
    private Camera _mainCam;
    
    // Item Data
    [Header("Quest Item")]
    [SerializeField] ItemType _itemNeeded;

    // Flavour Text
    [Header("Greeting Dialogue")]
    [SerializeField] string _greetingLion;
    [SerializeField] string _greetingHuman;
    string _greetingText;

    [Header("Quest Dialogue")]
    [SerializeField] TextAsset _introductionText;
    [SerializeField] TextAsset _noItemText;
    [SerializeField] TextAsset _itemFoundText;
    [SerializeField] TextAsset _conclusionText;
    
    // Dialogue flags
    bool _introDone;
    bool _hasItem;
    bool _infoGiven;
    bool _isTalking;

    // Events
    public static Func<ItemType, bool> CheckItemFound;
    public static Action<ItemType> ItemDeliveredEvent;
    public static Action<NPCEventData> SendNarrativeDataEvent;
    NPCEventData _npcEventData;

    void OnEnable()
    {
        HumanNarrativeState.StartNarrativeEvent += HideGreeting;
        NarrativeManager.EndOfNarrativeEvent += OnPlayerDialogueFinished;
    }

    void OnDisable()
    {
        HumanNarrativeState.StartNarrativeEvent -= HideGreeting;
        NarrativeManager.EndOfNarrativeEvent -= OnPlayerDialogueFinished;
    }

    void Start()
    {
        _mainCam = Camera.main;
        _interactUI.gameObject.SetActive(false);

        _hasItem = false;
        _introDone = false;
        _infoGiven = false;
        _isTalking = false;

        _interactText = _interactUI.GetComponentInChildren<TextMeshProUGUI>();

        SetDialogue();
        _npcEventData = new NPCEventData(false, false, this.transform, _introductionText, _colour);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, _mainCam.transform.rotation.eulerAngles.y, 0f);
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponentInParent<Player>().IsLion;

        // Find out what side of the player the NPC is on
        _npcEventData.IsOnRightSide = player.transform.position.x > transform.position.x ? false : true;
        
        // If player is not a lion
        if (!isLion)
        {
            if (!_hasItem)
            {
                CheckForFoundItem();
            }

            SetDialogue();

            _greetingText = _greetingHuman;
            _UIPromptText = _UIPromptHuman;

            _npcEventData.CanInteract = canInteract;
            SendNarrativeDataEvent?.Invoke(_npcEventData);
        }
        // If player is a lion
        else
        {
            _greetingText = _greetingLion;
            _UIPromptText = _UIPromptLion;
        }

        _isTalking = true;
        DisplayGreeting(canInteract);
        Interactable.InteractUIPromptEvent?.Invoke(_UIPromptText, canInteract);
    }

    void OnPlayerDialogueFinished()
    {
        if (_isTalking)
        {
            if (_npcEventData.CurrentDialogue == _introductionText)
            {
                Debug.Log("Intro done");
                _introDone = true;
            }
            else if (_npcEventData.CurrentDialogue == _itemFoundText)
            {
                _infoGiven = true;
                ItemDeliveredEvent?.Invoke(_itemNeeded);
            }
        }
       
       _isTalking = false;
    }

    void SetDialogue()
    {
        // Set to intro dialogue if first meeting NPC
        if (!_introDone && !_hasItem && !_infoGiven)
        {
            _npcEventData.CurrentDialogue = _introductionText;
        }
        // Set to no item dialogue if intro has been completed
        else if (_introDone && !_hasItem && !_infoGiven)
        {
            _npcEventData.CurrentDialogue = _noItemText;
        }
        // Set to has item dialogue if item is in inventory
        else if (_hasItem && !_infoGiven)
        {
            _npcEventData.CurrentDialogue = _itemFoundText;
            _introDone = true;
        }
        // Set to conclusion dialogue after receiving information
        else if (_hasItem && _infoGiven)
        {
            _npcEventData.CurrentDialogue = _conclusionText;
        }
    }

    void CheckForFoundItem()
    {
        if (!_hasItem)
        {
            _hasItem = CheckItemFound.Invoke(_itemNeeded);
        }
    }

    void DisplayGreeting(bool displayGreeting)
    {
        _interactText.text = _greetingText;
        _interactUI.gameObject.SetActive(displayGreeting);
    }

    void HideGreeting(bool displayGreeting)
    {
        if (_npcEventData.CanInteract && displayGreeting)
        {
            _interactText.text = _greetingText;
            _interactUI.gameObject.SetActive(!displayGreeting);
        }
    }
}
