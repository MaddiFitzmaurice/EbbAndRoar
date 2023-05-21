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
    bool _hasItemNeeded;

    // Flavour Text
    [Header("Greeting Dialogue")]
    [SerializeField] string _greetingLion;
    [SerializeField] string _greetingHuman;
    string _greetingText;

    [Header("Quest Dialogue")]
    [SerializeField] TextAsset _introductionText;

    // Events
    public static Action<ItemType> CheckItemFound;
    public static Action<NPCEventData> SendNarrativeDataEvent;
    NPCEventData _npcEventData;

    void OnEnable()
    {
        HumanNarrativeState.StartNarrativeEvent += HideGreeting;
    }

    void OnDisable()
    {
        HumanNarrativeState.StartNarrativeEvent -= HideGreeting;
    }

    void Start()
    {
        _mainCam = Camera.main;
        _interactUI.gameObject.SetActive(false);
        _hasItemNeeded = false;
        _interactText = _interactUI.GetComponentInChildren<TextMeshProUGUI>();
        _npcEventData = new NPCEventData(false, false, this.transform, _introductionText, _colour);
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, _mainCam.transform.rotation.eulerAngles.y, 0f);
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponent<Player>().IsLion;

        // Find out what side of the player the NPC is on
        _npcEventData.IsOnRightSide = player.transform.position.x > transform.position.x ? false : true;
        
        // If player is not a lion
        if (!isLion)
        {
            CheckForFoundItem();

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

        DisplayGreeting(canInteract);
        Interactable.InteractUIPromptEvent?.Invoke(_UIPromptText, canInteract);
    }

    void CheckForFoundItem()
    {
        if (!_hasItemNeeded)
        {
            CheckItemFound?.Invoke(_itemNeeded);
        }
    }

    void DisplayGreeting(bool displayGreeting)
    {
        _interactText.text = _greetingText;
        _interactUI.gameObject.SetActive(displayGreeting);
    }

    void HideGreeting(bool displayGreeting)
    {
        if (_npcEventData.CanInteract)
        {
            _interactText.text = _greetingText;
            _interactUI.gameObject.SetActive(!displayGreeting);
        }
    }
}
