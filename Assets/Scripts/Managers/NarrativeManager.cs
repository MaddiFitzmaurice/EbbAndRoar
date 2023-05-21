using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;

public struct NarrativeUIData
{
    public bool UsePanelRightSide;
    public string Dialogue;
    public Color PanelColour;

    public NarrativeUIData(bool panelRightSide, string dialogue, Color colour)
    {
        UsePanelRightSide = panelRightSide;
        Dialogue = dialogue;
        PanelColour = colour;
    }
}

public class NarrativeManager : MonoBehaviour
{
    // Current Narrative Panel Data
    Story _currentDialogue;
    bool _npcIsOnRightSide;
    Color _npcColour;
    Color _playerColour = new Color(61/255f, 23/255f, 143/255f);

    // Narrative Event Data
    NarrativeUIData _narrativeUIData;
    public static Action<NarrativeUIData> NarrativeUIEvent;
    public static Action EndOfNarrativeEvent;

    void OnEnable()
    {
        NPC.SendNarrativeDataEvent += NPCEventDataHandler;
        HumanNarrativeState.StartNarrativeEvent += StartDialogue;
        HumanNarrativeState.NarrativeInteractEvent += NextDialogue;
    }

    void OnDisable()
    {
        NPC.SendNarrativeDataEvent -= NPCEventDataHandler;
        HumanNarrativeState.StartNarrativeEvent -= StartDialogue;
        HumanNarrativeState.NarrativeInteractEvent -= NextDialogue;
    }

    void Start()
    {
        _narrativeUIData = new NarrativeUIData(false, "", _playerColour);
    }

    // Set up new dialogue for UI
    void NPCEventDataHandler(NPCEventData npcEventData)
    {
        _currentDialogue = new Story(npcEventData.CurrentDialogue.text);
        _npcIsOnRightSide = npcEventData.IsOnRightSide;
        _npcColour = npcEventData.Colour;
    }

    public void NextDialogue()
    {
        if (_currentDialogue.canContinue)
        {
            string line = _currentDialogue.Continue();
            SetDialogue(line);
            string speaker = HandleTag(_currentDialogue.currentTags);
            SetSpeaker(speaker);
            NarrativeUIEvent?.Invoke(_narrativeUIData);
        }
        else
        {
            EndOfNarrativeEvent?.Invoke();
        }
    }

    // Initiate dialogue
    void StartDialogue(bool start)
    {
        if (start)
        {
            NextDialogue();
        }
    }

    void SetDialogue(string dialogue)
    {
        _narrativeUIData.Dialogue = dialogue;
    }

    void SetSpeaker(string speaker)
    {
        // If player is speaking
        if (speaker == "purple")
        {
            _narrativeUIData.PanelColour = _playerColour;

            if (_npcIsOnRightSide)
            {
                _narrativeUIData.UsePanelRightSide = false;
            }
            else
            {
                _narrativeUIData.UsePanelRightSide = true;
            }
        }
        // If NPC is speaking
        else
        {
            _narrativeUIData.PanelColour = _npcColour;

            if (_npcIsOnRightSide)
            {
                _narrativeUIData.UsePanelRightSide = true;
            }
            else
            {
                _narrativeUIData.UsePanelRightSide = false;
            }
        }
    }
    
    public string HandleTag(List<string> currentTags)
    {
        if (currentTags.Count != 0)
        {
            string speaker = currentTags[0].Trim();

            if (speaker == "purple")
            {
                return "purple";
            }
            else if (speaker == "yellow")
            {
                return "yellow";
            }
            else if (speaker == "green")
            {
                return "green";
            }
            else if (speaker == "red")
            {
                return "red";
            }
        }

        return null;
    }
}
