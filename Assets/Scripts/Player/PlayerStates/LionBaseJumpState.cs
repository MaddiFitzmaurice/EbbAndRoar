using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class LionBaseJumpState : PlayerState
{    
    private float _jumpTimer;
    private float _bufferTime = 0.5f;

    public static Action<bool> JumpEvent;

    public LionBaseJumpState(Player player) : base(player) {}

    public override void Enter()
    {
        base.Enter();
        Jump();
        Physics.gravity = Player.GravityUp;
        JumpEvent?.Invoke(true);
        _jumpTimer = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {        
        _jumpTimer += Time.deltaTime;
        Player.IsGrounded = GroundCheck();
    }

    public override void PhysicsUpdate()
    {
        // If player is travelling up to height of jump
        if (Player.Rb.velocity.y > 0)
        {
            Physics.gravity = Player.GravityUp;
        }

        // If player has passed height of their jump
        if (Player.Rb.velocity.y < 0)
        {
            Physics.gravity = Player.GravityDown;            
        }

        // If player has landed
        if (Player.IsGrounded && _jumpTimer > _bufferTime)
        {
            Physics.gravity = Player.GravityNorm;
            JumpEvent?.Invoke(false);
            Player.StateMachine.ChangeState(Player.L_IdleState);
        }
    }

    protected abstract void Jump();
}
