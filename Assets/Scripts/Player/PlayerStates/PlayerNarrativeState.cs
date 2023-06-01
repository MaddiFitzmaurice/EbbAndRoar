using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerNarrativeState : BaseState
{
    Player _player;
    bool _canPress;
    public static Action<bool> StartNarrativeEvent;
    public static Action NarrativeInteractEvent;

    public PlayerNarrativeState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
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
        NarrativeManager.EndOfNarrativeEvent -= ExitNarrativeState;
        UIManager.CanPressContinueEvent -= CanPressContinueEventHandler;

        StartNarrativeEvent?.Invoke(false);
    }

    void ExitNarrativeState()
    {
        if (!_player.IsLion)
        {
            _player.StateMachine.ChangeState(_player.H_MoveState);
        }
        else 
        {
            _player.StateMachine.ChangeState(_player.L_IdleState);
        }
    }

    void CanPressContinueEventHandler(bool canPress)
    {
        _canPress = canPress;
    }
}
