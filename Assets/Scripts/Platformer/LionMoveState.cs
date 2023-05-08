using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LionMoveState : PlayerMoveState
{    
    public LionMoveState(PlayerLion player) : base(player)
    {
       
    }

    public override void Enter()
    {
        
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
            PlayerLion player = Player as PlayerLion;
            Player.StateMachine.ChangeState(player.JumpState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
