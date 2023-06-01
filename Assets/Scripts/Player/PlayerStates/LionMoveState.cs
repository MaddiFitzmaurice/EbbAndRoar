using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LionMoveState : PlayerState
{   
    bool _isFalling;
     
    public LionMoveState(Player player) : base(player) {}

    public override void Enter()
    {
        base.Enter();
        _isFalling = false;
        ChangeForms(FormType.Lion);
    }

    public override void Exit() 
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        IsFalling();

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

    void IsFalling()
    {
        if (Player.Rb.velocity.y < -1f && !_isFalling && !GroundCheck())
        {
            _isFalling = true;
            Player.Sprite.sprite = Player.LionIdleJumpDownSprite;
        }
        else if (Player.Rb.velocity.y >= -1f && _isFalling && GroundCheck()) 
        {
            _isFalling = false;
            Player.Sprite.sprite = Player.LionSprite;
        }
    }
}
