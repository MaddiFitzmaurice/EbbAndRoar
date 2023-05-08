using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveState : BaseState
{
    Player _player;

    private float _xInput;
    private bool _isFacingRight;

    // Events
    public static Action<bool> DirectionChangeEvent;
    
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

        // Check facing direction and update camera blend
        if (_xInput == 1)
        {
            _isFacingRight = true;
        }
        else if (_xInput == -1)
        {
            _isFacingRight = false;
        }

        DirectionChangeEvent?.Invoke(_isFacingRight);

        // Jump 
        if (Input.GetButtonDown("Jump") && _player.Rb.velocity.y == 0)
        {
            _player.StateMachine.ChangeState(_player.JumpState);
        }
    }
}
