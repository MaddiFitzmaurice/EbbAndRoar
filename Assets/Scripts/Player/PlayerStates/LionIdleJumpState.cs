using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionIdleJumpState : LionBaseJumpState
{
    Vector3 _idleJumpDir = new Vector3(0.2f, 1f, 0f);

    public LionIdleJumpState(Player player) : base (player) {}

    public override void Enter()
    {
        ChangeForms(FormType.UpJump);
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Player.Rb.velocity.y < 0.1f && !GroundCheck())
        {
            ChangeForms(FormType.DownJump);           
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void Jump()
    {
        if (Player.IsFacingRight)
        {
            _idleJumpDir.x = Mathf.Abs(_idleJumpDir.x);
        }
        else
        {
            _idleJumpDir.x = Mathf.Abs(_idleJumpDir.x) * -1;
        }

        float jumpForce = Mathf.Sqrt(Player.IdleJumpHeight * Physics.gravity.y * -2) * Player.Rb.mass;
        Player.Rb.AddForce(_idleJumpDir * jumpForce, ForceMode.Impulse);
    }
}
