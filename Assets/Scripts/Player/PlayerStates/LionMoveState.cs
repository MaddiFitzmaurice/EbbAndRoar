using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LionMoveState : PlayerState
{    
    public LionMoveState(Player player) : base(player) {}

    public override void Enter()
    {
        base.Enter();
        ChangeForms(FormType.Lion);
    }

    public override void Exit() 
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        // Jump 
        if (Input.GetButtonDown("Jump") && Player.IsGrounded)
        {
            Player.StateMachine.ChangeState(Player.L_MoveJumpState);
        }
        // Change to idle if no input
        else if (Mathf.Abs(Player.Rb.velocity.x) < 0.01f && Player.XInput == 0)
        {
            Player.StateMachine.ChangeState(Player.L_IdleState);
        }

        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
