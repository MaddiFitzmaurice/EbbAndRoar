using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _promptText;

    [SerializeField] List<Image> _itemsUI;
    List<Item> _items;
    Vector4 _transparent = new Vector4(0, 0, 0, 50);

    void Start()
    {
        _promptText.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Interactable.InteractUIPromptEvent += UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent += UpdateUIItemsList;
    }

    void OnDisable()
    {
        Interactable.InteractUIPromptEvent -= UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent -= UpdateUIItemsList;
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
