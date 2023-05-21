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
    #endregion

    [Header("Narrative Dialogue Speed")]
    [SerializeField] float _dialogueSpeed;
    Coroutine _currentCoroutine;

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
    }

    void DisplayNarrativeUIPanel(bool isActive)
    {
        _narrativeUIPanel.SetActive(isActive);
        _itemsUIPanel.SetActive(!isActive);

        UpdatePromptUI("Press E to continue.", isActive);
        
    }

    // Set panel data
    void UpdateNarrativeUIPanel(NarrativeUIData data)
    {
        // Stop previous typing effect coroutine if player skips
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        DialoguePanel panel;

        if (data.UsePanelRightSide)
        {
            panel = _rightDialoguePanel;
        }
        else
        {
           panel = _leftDialoguePanel;
        }

        panel.Image.color = new Color(data.PanelColour.r, data.PanelColour.g, data.PanelColour.b, 0.4f);
        //panel.Text.text = data.Dialogue;
        ShowPanel(data.UsePanelRightSide);
        _currentCoroutine = StartCoroutine(TypingEffect(data.Dialogue, panel));
    }

    // Narrative UI Panel Functions
    private IEnumerator TypingEffect(string line, DialoguePanel panel)
    {
        panel.Text.text = "";
        int visibleChars = 0;

        foreach (char letter in line.ToCharArray())
        {
            panel.Text.text += letter;
            //PlayTalkingAudio(visibleChars, letter);
            visibleChars++;
            yield return new WaitForSeconds(_dialogueSpeed);
        }
    } 

    void ShowPanel(bool isRightPanel)
    {
        _leftDialoguePanel.Panel.SetActive(!isRightPanel);
        _rightDialoguePanel.Panel.SetActive(isRightPanel);
    }

    void ResetNarrativePanels()
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
