using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LionMoveState : PlayerState
{   
    bool _isFalling;
    float _timer;
     
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

        if (!_isFalling)
        {
            WalkCycle();
        }

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
            ChangeForms(FormType.DownJump);
        }
        else if (Player.Rb.velocity.y >= -1f && _isFalling && GroundCheck()) 
        {
            _isFalling = false;
            ChangeForms(FormType.Lion);
        }
    }

    void WalkCycle()
    {
        if (_timer > Player.WalkCycleTime)
        {
            _timer = 0;
            ChangeWalkSprite();
        }
        else 
        {
            _timer += Time.deltaTime;
        }
    }

    void ChangeWalkSprite()
    {
        if (Player.Sprite.sprite == Player.LionSprite)
        {
            Player.Sprite.sprite = Player.LionWalkingSprite;
        }
        else 
        {
            Player.Sprite.sprite = Player.LionSprite;
        }
    }
}
