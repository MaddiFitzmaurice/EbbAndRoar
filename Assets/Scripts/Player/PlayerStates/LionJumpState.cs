using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionJumpState : PlayerMoveState
{    
    public LionJumpState(Player player) : base(player)
    {

    }

    public override void Enter()
    {
        Jump();
        Debug.Log("Jump state");
    }

    public override void LogicUpdate()
    {        
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        if (Player.IsGrounded)
        {
            Player.StateMachine.ChangeState(Player.L_MoveState);
        }
        base.PhysicsUpdate();
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt(Player.JumpHeight * Physics.gravity.y * -2) * Player.Rb.mass;
        Player.Rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
