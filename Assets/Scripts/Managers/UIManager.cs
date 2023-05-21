using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public struct DialoguePanel
{
    public GameObject Panel;
    public TextMeshProUGUI Text;
    public Image Image;

    public DialoguePanel(GameObject panel, TextMeshProUGUI text, Image image)
    {
        Panel = panel;
        Text = text;
        Image = image;
    }
}

public class UIManager : MonoBehaviour
{
    #region UI Panels
    [Header("UI Panels")]
    [SerializeField] GameObject _gameUIPanel;
    [SerializeField] GameObject _narrativeUIPanel;
    #endregion

    #region Gameplay UI
    // Gameplay UI
    [Header("Gameplay UI Elements")]
    [SerializeField] TextMeshProUGUI _promptText;
    [SerializeField] List<Image> _itemsUI;
    #endregion

    #region Narrative UI Elements
    // Narrative UI
    [Header("Narrative UI Elements")]
    [SerializeField] GameObject _rightDialogueObj;
    [SerializeField] GameObject _leftDialogueObj;
    DialoguePanel _rightDialoguePanel;
    DialoguePanel _leftDialoguePanel;
    #endregion

    // Item UI
    List<Item> _items;

    #region Transparency Vectors
    // UI Transparency
    Vector4 _itemTransparency = new Vector4(0f, 0f, 0f, 50f);
    Vector4 _panelTransparency = new Vector4(0f, 0f, 0f, 100f);
    #endregion

    void Start()
    {
        _promptText.gameObject.SetActive(false);
        _narrativeUIPanel.SetActive(false);
        _gameUIPanel.SetActive(true);   

        SetupDialoguePanels();
    }

    void OnEnable()
    {
        Interactable.InteractUIPromptEvent += UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent += UpdateUIItemsList;
        HumanNarrativeState.StartNarrativeEvent += DisplayNarrativeUIPanel;
        NarrativeManager.NarrativeUIEvent += UpdateNarrativeUIPanel;
    }

    void OnDisable()
    {
        Interactable.InteractUIPromptEvent -= UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent -= UpdateUIItemsList;
        HumanNarrativeState.StartNarrativeEvent -= DisplayNarrativeUIPanel;
        NarrativeManager.NarrativeUIEvent += UpdateNarrativeUIPanel;
    }

    void SetupDialoguePanels()
    {
        Image leftImage = _leftDialogueObj.GetComponent<Image>();
        Image rightImage = _rightDialogueObj.GetComponent<Image>();
        TextMeshProUGUI leftText = _leftDialogueObj.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI rightText = _rightDialogueObj.GetComponentInChildren<TextMeshProUGUI>();

        _leftDialoguePanel = new DialoguePanel(_leftDialogueObj, leftText, leftImage);
        _rightDialoguePanel = new DialoguePanel(_rightDialogueObj, rightText, rightImage);
    }

    void DisplayNarrativeUIPanel(bool isActive)
    {
        _narrativeUIPanel.SetActive(isActive);
        _gameUIPanel.SetActive(!isActive);
    }

    // Set panel data
    void UpdateNarrativeUIPanel(NarrativeUIData data)
    {
        ShowPanel(data.UsePanelRightSide);

        DialoguePanel panel;

        if (data.UsePanelRightSide)
        {
            panel = _rightDialoguePanel;
        }
        else
        {
           panel = _leftDialoguePanel;
        }

        panel.Image.color = (Vector4)data.PanelColour - _panelTransparency;
        panel.Text.text = data.Dialogue;
    }

    void ShowPanel(bool isRightPanel)
    {
        _leftDialoguePanel.Panel.SetActive(!isRightPanel);
        _rightDialoguePanel.Panel.SetActive(isRightPanel);
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
                _itemsUI[i].color = _itemTransparency;
            }
        }
    }
}
