using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : BaseState
{
    Player _player;

    private float _xInput;

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
        _player.Movement(_xInput, _player.MSpeed, _player.MAcceleration,
            _player.MDecceleration, _player.MVelocityPower);
    }

    void PlayerInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");

        // Jump 
        if (Input.GetButtonDown("Jump") && _player.Rb.velocity.y == 0)
        {
            _player.StateMachine.ChangeState(_player.JumpState);
        }
    }
}
