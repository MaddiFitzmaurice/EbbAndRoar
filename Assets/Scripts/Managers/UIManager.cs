using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _promptText;

    [SerializeField] List<Image> _items;
    List<UIItem> _UIItems;

    private struct UIItem 
    {
        public bool ItemFound;
        public Image Image;

        public UIItem(bool itemFound, Image image)
        {
            this.ItemFound = itemFound;
            this.Image = image;
        }
    }

    void Start()
    {
        _promptText.gameObject.SetActive(false);
        CreateUIItemsList();
    }

    void OnEnable()
    {
        Interactable.InteractUIPromptEvent += UpdatePromptUI;
        Item.ItemPickupEvent += UpdateUIItemsList;
    }

    void OnDisable()
    {
        Interactable.InteractUIPromptEvent -= UpdatePromptUI;
        Item.ItemPickupEvent -= UpdateUIItemsList;
    }

    void UpdatePromptUI(string prompt, bool showPrompt)
    {
        _promptText.gameObject.SetActive(showPrompt);
        _promptText.text = prompt;
    }

    void CreateUIItemsList()
    {
        _UIItems = new List<UIItem>();

        foreach (Image image in _items)
        {
            _UIItems.Add(new UIItem(false, image));
        }
    }

    void UpdateUIItemsList(Item item)
    {
        foreach (UIItem uiItem in _UIItems)
        {
            if (!uiItem.ItemFound)
            {
                uiItem.Image.sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
                uiItem.Image.color = Color.white;
                break;
            }
        }
    }
}
