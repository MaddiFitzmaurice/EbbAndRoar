using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Ink.Runtime;
using System;

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
    [SerializeField] GameObject _itemsUIPanel;
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
    [SerializeField] GameObject _choice1Obj;
    [SerializeField] GameObject _choice2Obj;
    TextMeshProUGUI _choice1Text;
    TextMeshProUGUI _choice2Text;
    #endregion

    [Header("Narrative Dialogue Speed")]
    [SerializeField] float _dialogueSpeed;
    Coroutine _currentCoroutine;
    public static Action<bool> CanPressContinueEvent;

    // Item UI
    List<Item> _items;

    #region Transparency Vectors
    // UI Transparency
    Vector4 _itemTransparency = new Vector4(0f, 0f, 0f, 50f);
    #endregion

    void Start()
    {
        _promptText.gameObject.SetActive(false);
        _narrativeUIPanel.SetActive(false);
        _itemsUIPanel.SetActive(true);   

        SetupDialoguePanels();
    }

    void OnEnable()
    {
        Interactable.InteractUIPromptEvent += UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent += UpdateUIItemsList;
        HumanNarrativeState.StartNarrativeEvent += DisplayNarrativeUIPanel;
        NarrativeManager.NarrativeUIEvent += UpdateNarrativeUIPanel;
        NarrativeManager.EndOfNarrativeEvent += ResetNarrativePanels;
    }

    void OnDisable()
    {
        Interactable.InteractUIPromptEvent -= UpdatePromptUI;
        ItemManager.UpdateItemsCollectedEvent -= UpdateUIItemsList;
        HumanNarrativeState.StartNarrativeEvent -= DisplayNarrativeUIPanel;
        NarrativeManager.NarrativeUIEvent -= UpdateNarrativeUIPanel;
        NarrativeManager.EndOfNarrativeEvent -= ResetNarrativePanels;
    }

    void SetupDialoguePanels()
    {
        Image leftImage = _leftDialogueObj.GetComponent<Image>();
        Image rightImage = _rightDialogueObj.GetComponent<Image>();
        TextMeshProUGUI leftText = _leftDialogueObj.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI rightText = _rightDialogueObj.GetComponentInChildren<TextMeshProUGUI>();

        _leftDialoguePanel = new DialoguePanel(_leftDialogueObj, leftText, leftImage);
        _rightDialoguePanel = new DialoguePanel(_rightDialogueObj, rightText, rightImage);

        _choice1Text = _choice1Obj.GetComponentInChildren<TextMeshProUGUI>();
        _choice2Text = _choice2Obj.GetComponentInChildren<TextMeshProUGUI>();
    }

    void DisplayNarrativeUIPanel(bool isActive)
    {
        _narrativeUIPanel.SetActive(isActive);
        _itemsUIPanel.SetActive(!isActive);

        // Deactivate prompt UI until player can press continue
        UpdatePromptUI("", false);
    }

    void DisplayChoices(List<Choice> choices, bool display)
    {
        if (display)
        {
            _choice1Text.text = choices[0].text;
            _choice2Text.text = choices[1].text;

            UpdatePromptUI("W/S to select. E to confirm.", true);
        }
        else 
        {
            UpdatePromptUI("Press E to continue.", true);
        }

        _choice1Obj.SetActive(display);
        _choice2Obj.SetActive(display);
    }

    // Set panel data
    void UpdateNarrativeUIPanel(NarrativeUIData data)
    {
        ResetChoicePanels();
        
        // Stop previous typing effect coroutine if player skips
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        // Decide what panel to use
        DialoguePanel panel = data.UsePanelRightSide == true ? _rightDialoguePanel : _leftDialoguePanel;

        // Set text and colour of panel, then show chosen panel and start type effect
        panel.Image.color = new Color(data.PanelColour.r, data.PanelColour.g, data.PanelColour.b, 0.4f);
        ShowDialoguePanel(data.UsePanelRightSide);
        _currentCoroutine = StartCoroutine(TypeDialogue(data, panel));
    }

    // Narrative UI Panel Functions
    private IEnumerator TypeDialogue(NarrativeUIData data, DialoguePanel panel)
    {
        // Stop player from pressing continue until dialogue is finished
        CanPressContinueEvent?.Invoke(false);
        UpdatePromptUI("", false);

        panel.Text.text = "";
        int visibleChars = 0;

        // Print out characters one by one
        foreach (char letter in data.Dialogue.ToCharArray())
        {
            panel.Text.text += letter;
            //PlayTalkingAudio(visibleChars, letter);
            visibleChars++;
            yield return new WaitForSeconds(_dialogueSpeed);
        }

        // Allow player to press continue/select choice
        CanPressContinueEvent?.Invoke(true);

        // If choices are available, display them after typing has finished
        bool hasChoice = data.Choices.Count != 0 ? true : false;
        DisplayChoices(data.Choices, hasChoice);
    } 

    // Show appropriate dialogue panel (left or right one)
    void ShowDialoguePanel(bool isRightPanel)
    {
        _leftDialoguePanel.Panel.SetActive(!isRightPanel);
        _rightDialoguePanel.Panel.SetActive(isRightPanel);
    }

    // Reset all narrative panels and deactivate them
    void ResetNarrativePanels()
    {
        ResetDialoguePanels();
        ResetChoicePanels();
    }

    void ResetChoicePanels()
    {
        _choice1Text.text = "";
        _choice2Text.text = "";
        _choice1Obj.SetActive(false);
        _choice2Obj.SetActive(false);
    }

    void ResetDialoguePanels()
    {
        _leftDialoguePanel.Text.text = "";
        _rightDialoguePanel.Text.text = "";
        _leftDialoguePanel.Panel.SetActive(false);
        _rightDialoguePanel.Panel.SetActive(false);
    }
    // Game UI Panel Functions
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
