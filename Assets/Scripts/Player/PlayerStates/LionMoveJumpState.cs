using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionMoveJumpState : LionBaseJumpState
{
    Vector3 _moveJumpDir = new Vector3(1f, 1f, 0);
    public LionMoveJumpState(Player player) : base (player) {}

    public override void Enter()
    {   
        ChangeForms(FormType.Leap);
        Debug.Log("Move Jump");
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void Jump()
    {
        if (Player.IsFacingRight)
        {
            _moveJumpDir.x = Mathf.Abs(_moveJumpDir.x);
        }
        else
        {
            _moveJumpDir.x = Mathf.Abs(_moveJumpDir.x) * -1;
        }

        float jumpForce = Mathf.Sqrt(Player.MoveJumpHeight * Physics.gravity.y * -2) * Player.Rb.mass;
        Player.Rb.AddForce(_moveJumpDir * jumpForce, ForceMode.Impulse);
    }
}
