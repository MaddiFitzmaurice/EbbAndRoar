using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanNarrativeState : BaseState
{
    Player _player;
    public static Action<bool> NarrativeEvent;

    public HumanNarrativeState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
        Debug.Log("Entered narrative state");
        NarrativeEvent?.Invoke(true);
    }

    public override void LogicUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _player.StateMachine.ChangeState(_player.H_MoveState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited narrative state");
        NarrativeEvent?.Invoke(false);
    }
}
