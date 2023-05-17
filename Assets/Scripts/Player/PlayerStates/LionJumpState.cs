using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LionJumpState : PlayerMoveState
{    
    private bool _applyDownForce;
    private float _jumpTimer;
    private float _bufferTime = 0.5f;

    public static Action<bool> JumpEvent;
    bool _isJumping;

    public LionJumpState(Player player) : base(player)
    {

    }

    public override void Enter()
    {
        Jump();
        _isJumping = true;
        JumpEvent?.Invoke(_isJumping);
        _jumpTimer = 0;
        _applyDownForce = true;
        Debug.Log("Jump State");
    }

    public override void LogicUpdate()
    {        
        base.LogicUpdate();
        _jumpTimer += Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        // If player has landed
        if (Player.IsGrounded && _jumpTimer > _bufferTime)
        {
            _isJumping = false;
            JumpEvent?.Invoke(_isJumping);
            Player.StateMachine.ChangeState(Player.L_MoveState);
        }

        // If player has passed height of their jump
        if (Player.Rb.velocity.y < 0 && _applyDownForce)
        {
            Debug.Log("Falling Down");
            //Player.Rb.AddForce(Vector3.down * 200f, ForceMode.Acceleration);
            _applyDownForce = false;
        }

        base.PhysicsUpdate();
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt(Player.JumpHeight * Physics.gravity.y * -2) * Player.Rb.mass;
        Player.Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
