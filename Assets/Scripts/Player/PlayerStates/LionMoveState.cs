using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LionMoveState : PlayerMoveState
{    
    public LionMoveState(Player player) : base(player)
    {
       
    }

    public override void Enter()
    {
        Player.CurrentData = Player.LionData;
        Player.IsLion = true;
        Debug.Log("Lion");
    }

    public override void Exit()
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Jump 
        if (Input.GetButtonDown("Jump") && Player.Rb.velocity.y == 0)
        {
            Player.StateMachine.ChangeState(Player.L_JumpState);
        }

        if (Player.LionTimer >= Player.LionTime)
        {
            Player.StateMachine.ChangeState(Player.H_MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
