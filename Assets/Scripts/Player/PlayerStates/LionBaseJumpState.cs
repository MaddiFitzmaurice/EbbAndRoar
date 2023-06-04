using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class LionBaseJumpState : PlayerState
{    
    protected float JumpTimer;
    protected float BufferTime = 0.5f;

    public static Action<bool> JumpEvent;

    public LionBaseJumpState(Player player) : base(player) {}

    public override void Enter()
    {
        base.Enter();
        Jump();
        Physics.gravity = Player.GravityUp;
        JumpEvent?.Invoke(true);
        JumpTimer = 0;
    }

    public override void Exit()
    {
        ChangeForms(FormType.Lion);
        base.Exit();
    }

    public override void LogicUpdate()
    {        
        JumpTimer += Time.deltaTime;
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

        
    }

    protected abstract void Jump();
}
