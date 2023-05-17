using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject _interactUI;
    private Camera _mainCam;
    
    [SerializeField] ItemType _itemNeeded;
    bool _hasItemNeeded;

    public static Action<ItemType> CheckItemFound;

    void Start()
    {
        _mainCam = Camera.main;
        _interactUI.gameObject.SetActive(false);
        _hasItemNeeded = false;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, _mainCam.transform.rotation.eulerAngles.y, 0f);
    }

    public void OnPlayerInteract(Collider player, bool canInteract)
    {
        bool isLion = player.GetComponent<Player>().IsLion;
        
        if (!isLion)
        {
            if (!_hasItemNeeded)
            {
                CheckItemFound?.Invoke(_itemNeeded);
            }

            _interactUI.gameObject.SetActive(canInteract);
            Interactable.InteractUIPromptEvent?.Invoke("Press E to talk.", canInteract);
        }
    }
}
