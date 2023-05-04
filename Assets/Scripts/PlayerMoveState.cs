using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : BaseState
{
    Player _player;
    public PlayerMoveState(Player player)
    {
        _player = player;
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void LogicUpdate()
    {
        PlayerInput();
    }

    public override void PhysicsUpdate()
    {
        PlayerMovement();
    }

    void PlayerInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        _player.MoveDir = Vector3.right * xInput;

        // Jump 
        if (Input.GetButtonDown("Jump") && _player.Rb.velocity.y == 0)
        {
            _player.StateMachine.ChangeState(_player.JumpState);
        }
    }

    void PlayerMovement()
    {
        if (_player.Rb.velocity.magnitude < _player.TargetVelocity)
        {
            _player.Rb.AddForce(_player.MoveDir * _player.MoveForce, ForceMode.Acceleration);
        }
    }
}
