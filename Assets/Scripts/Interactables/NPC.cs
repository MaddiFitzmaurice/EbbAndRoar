using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class NPC : MonoBehaviour, Interactable
{
    // Interaction
    [SerializeField] private GameObject _interactUI;
    private TextMeshProUGUI _interactText;
    [SerializeField] string _UIPromptLion;
    [SerializeField] string _UIPromptHuman;
    string _UIPromptText;
    
    // Billboarding
    private Camera _mainCam;
    
    // Item Data
    [SerializeField] ItemType _itemNeeded;
    bool _hasItemNeeded;

    // Flavour Text
    [SerializeField] string _greetingLion;
    [SerializeField] string _greetingHuman;
    string _greetingText;

    public static Action<ItemType> CheckItemFound;

    void Start()
    {
        _mainCam = Camera.main;
        _interactUI.gameObject.SetActive(false);
        _hasItemNeeded = false;
        _interactText = _interactUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, _mainCam.transform.rotation.eulerAngles.y, 0f);
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponent<Player>().IsLion;
        
        // If player is not a lion
        if (!isLion)
        {
            CheckForFoundItem();

            _greetingText = _greetingHuman;
            _UIPromptText = _UIPromptHuman;
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
}
