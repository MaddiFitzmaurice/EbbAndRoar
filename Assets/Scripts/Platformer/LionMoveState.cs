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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
