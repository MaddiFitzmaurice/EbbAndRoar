using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] GameObject _gameUIPanel;
    [SerializeField] GameObject _narrativeUIPanel;

    // Gameplay UI
    [Header("Gameplay UI")]
    [SerializeField] TextMeshProUGUI _promptText;
    [SerializeField] List<Image> _itemsUI;
    
    // Item UI
    List<Item> _items;
    Vector4 _transparent = new Vector4(0, 0, 0, 50);

    void Start()
    {
        _promptText.gameObject.SetActive(false);
        _narrativeUIPanel.SetActive(false);
        _gameUIPanel.SetActive(false);   
    }

    void OnEnable()
    {
        Interactable.InteractUIPromptEvent += UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent += UpdateUIItemsList;
        HumanNarrativeState.NarrativeEvent += DisplayNarrativeUIPanel;
    }

    void OnDisable()
    {
        Interactable.InteractUIPromptEvent -= UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent -= UpdateUIItemsList;
        HumanNarrativeState.NarrativeEvent -= DisplayNarrativeUIPanel;
    }

    void DisplayNarrativeUIPanel(bool isActive)
    {
        _narrativeUIPanel.SetActive(isActive);
        _gameUIPanel.SetActive(!isActive);
    }

    void UpdatePromptUI(string prompt, bool showPrompt)
    {
        _promptText.gameObject.SetActive(showPrompt);
        _promptText.text = prompt;
    }

    void UpdateUIItemsList(List<Item> items)
    {
        _items = items;

        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Found)
            {
                _itemsUI[i].sprite = _items[i].Sprite;
                _itemsUI[i].color = Color.white;
            }

            if (_items[i].Delivered)
            {
                _itemsUI[i].color = _transparent;
            }
        }
    }
}
