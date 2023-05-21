using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanNarrativeState : BaseState
{
    Player _player;
    public static Action<bool> StartNarrativeEvent;
    public static Action NarrativeInteractEvent;

    public HumanNarrativeState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
        Debug.Log("Entered narrative state");
        StartNarrativeEvent?.Invoke(true);
    }

    public override void LogicUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NarrativeInteractEvent?.Invoke();
            //ExitNarrativeState();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exited narrative state");
        StartNarrativeEvent?.Invoke(false);
    }

    void ExitNarrativeState()
    {
            _player.StateMachine.ChangeState(_player.H_MoveState);
    }
}
