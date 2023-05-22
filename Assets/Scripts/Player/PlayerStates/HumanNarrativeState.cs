using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanNarrativeState : BaseState
{
    Player _player;
    bool _canPress;
    public static Action<bool> StartNarrativeEvent;
    public static Action NarrativeInteractEvent;

    public HumanNarrativeState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
        Debug.Log("Entered narrative state");
        NarrativeManager.EndOfNarrativeEvent += ExitNarrativeState;
        UIManager.CanPressContinueEvent += CanPressContinueEventHandler;

        StartNarrativeEvent?.Invoke(true);
    }

    public override void LogicUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && _canPress)
        {
            NarrativeInteractEvent?.Invoke();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited narrative state");
        NarrativeManager.EndOfNarrativeEvent -= ExitNarrativeState;
        UIManager.CanPressContinueEvent -= CanPressContinueEventHandler;

        StartNarrativeEvent?.Invoke(false);
    }

    void ExitNarrativeState()
    {
        _player.StateMachine.ChangeState(_player.H_MoveState);
    }

    void CanPressContinueEventHandler(bool canPress)
    {
        _canPress = canPress;
    }
}
