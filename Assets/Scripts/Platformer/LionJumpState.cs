using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionJumpState : PlayerMoveState
{    
    public LionJumpState(PlayerLion player) : base(player)
    {

    }

    public override void Enter()
    {
        Jump();
    }

    public override void LogicUpdate()
    {
        if (Physics.BoxCast(Player.transform.position, Player.Collider.bounds.extents, Vector3.down,
            out RaycastHit hit, Player.transform.rotation, 0.1f))
        {
            Debug.Log("landed");
            if (hit.collider.tag == "Ground")
            {
                PlayerLion player = Player as PlayerLion;
                Player.StateMachine.ChangeState(player.MoveState);
            }
        }
        else
        {
            Debug.Log("In air");
            base.LogicUpdate();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void Jump()
    {
        PlayerLion player = Player as PlayerLion;
        float jumpForce = Mathf.Sqrt(player.JumpHeight * Physics.gravity.y * -2) * Player.Rb.mass;
        Player.Rb.AddForce((Vector3.up + Vector3.forward).normalized * jumpForce, ForceMode.Impulse);
    }
}
