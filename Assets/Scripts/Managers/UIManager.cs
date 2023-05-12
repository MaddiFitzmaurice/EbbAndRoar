using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _promptText;

    void Start()
    {
        _promptText.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Interactable.InteractUIPromptEvent += UpdatePromptUI;
    }

    void OnDisable()
    {
        Interactable.InteractUIPromptEvent -= UpdatePromptUI;
    }

    void UpdatePromptUI(string prompt, bool showPrompt)
    {
        _promptText.gameObject.SetActive(showPrompt);
        _promptText.text = prompt;
    }
}
