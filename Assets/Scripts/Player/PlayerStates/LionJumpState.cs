using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionJumpState : PlayerMoveState
{    
    private bool _applyDownForce;
    private float _jumpTimer;
    private float _bufferTime = 0.5f;

    public LionJumpState(Player player) : base(player)
    {

    }

    public override void Enter()
    {
        Jump();
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
        if (Player.IsGrounded && _jumpTimer > _bufferTime)
        {
            Debug.Log("Changing");
            Player.StateMachine.ChangeState(Player.L_MoveState);
        }

        if (Player.Rb.velocity.y < 0 && _applyDownForce)
        {
            Debug.Log("Down");
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
