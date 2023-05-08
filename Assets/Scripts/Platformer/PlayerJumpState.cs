using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : BaseState
{
    Player _player;
    
    private float _xInput;

    public PlayerJumpState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
        Jump();
    }

    public override void LogicUpdate()
    {
        if (Physics.BoxCast(_player.transform.position, _player.Collider.bounds.extents * 2, Vector3.down,
            out RaycastHit hit, _player.transform.rotation, 0.1f) && _player.Rb.velocity.y < 0.01f)
        {
            if (hit.collider.tag == "Ground")
            {
                _player.StateMachine.ChangeState(_player.MoveState);
            }
        }
        else
        {
            _xInput = Input.GetAxisRaw("Horizontal");
        }
    }

    public override void PhysicsUpdate()
    {
        _player.Movement(_xInput, _player.JSpeed, _player.JAcceleration, _player.JDecceleration,
            _player.JVelocityPower);
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt(_player.JumpHeight * Physics.gravity.y * -2) * _player.Rb.mass;
        _player.Rb.AddForce((Vector3.up + Vector3.forward).normalized * jumpForce, ForceMode.Impulse);
    }
}
