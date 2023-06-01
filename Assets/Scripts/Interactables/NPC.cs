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
    [SerializeField] private GameObject _greetingUI;
    [SerializeField] private GameObject _questUI;
    [SerializeField] private GameObject _interactUI;
    private TextMeshProUGUI _greetingText;
    
    // Billboarding
    private Camera _mainCam;
    
    // Item Data
    [Header("Quest Item")]
    [SerializeField] ItemType _itemNeeded;

    // Flavour Text
    [Header("Greeting Lion Dialogue")]
    [SerializeField] string _greetingLionText;

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
        PlayerNarrativeState.StartNarrativeEvent += HideGreeting;
        NarrativeManager.EndOfNarrativeEvent += OnPlayerDialogueFinished;
    }

    void OnDisable()
    {
        PlayerNarrativeState.StartNarrativeEvent -= HideGreeting;
        NarrativeManager.EndOfNarrativeEvent -= OnPlayerDialogueFinished;
    }

    void Start()
    {
        _mainCam = Camera.main;
        
        UISetup();
        ResetFlags();
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

            _interactUI.SetActive(canInteract);
            _npcEventData.CanInteract = canInteract;
            SendNarrativeDataEvent?.Invoke(_npcEventData);
        }
        // If player is a lion
        else
        {
            _greetingUI.SetActive(canInteract);
        }

        _isTalking = canInteract;
        DisplayGreeting(canInteract);
    }

    void OnPlayerDialogueFinished()
    {
        if (_isTalking)
        {
            // Signals introduction has been completed
            if (_npcEventData.CurrentDialogue == _introductionText)
            {
                _introDone = true;
            }
            // Signals quest has been completed
            else if (_npcEventData.CurrentDialogue == _itemFoundText)
            {
                _infoGiven = true;
                ItemDeliveredEvent?.Invoke(_itemNeeded);
                _questUI.SetActive(false);
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
        // Deactivate quest icon and reactivate if quest has not been completed
        if (!_infoGiven)
        {
            _questUI.gameObject.SetActive(!displayGreeting);
        }
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

    void ResetFlags()
    {
        _hasItem = false;
        _introDone = false;
        _infoGiven = false;
        _isTalking = false;
    }

    void UISetup()
    {
        _greetingUI.gameObject.SetActive(false);
        _questUI.gameObject.SetActive(true);
        _greetingText = _greetingUI.GetComponentInChildren<TextMeshProUGUI>();
        _greetingText.text = _greetingLionText;
    }
}
