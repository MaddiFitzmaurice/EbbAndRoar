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
        PlayerMovement();
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

    void PlayerMovement()
    {
        // Calculate desired velocity
        float targetVelocity = _xInput * _player.Speed;

        // Find diff between desired velocity and current velocity
        float velocityDif = targetVelocity - _player.Rb.velocity.x;

        // Check whether to accel or deccel
        float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? _player.Acceleration :
            _player.Decceleration;

        // Calc force by multiplying accel and velocity diff, and applying velocity power
        float movement = Mathf.Pow(Mathf.Abs(velocityDif) * accelRate, _player.VelocityPower)
            * Mathf.Sign(velocityDif);

        _player.Rb.AddForce(movement * Vector3.right);
    }
}
