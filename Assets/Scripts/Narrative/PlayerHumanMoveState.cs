using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class PlayerHumanMoveState : BaseState
{
    PlayerHuman _player;

    float _xInput;

    bool _onPath;
    bool _canMoveY;
    PathTrigger _path;

    public PlayerHumanMoveState(PlayerHuman player)
    {
        _player = player;
    }

    public override void Enter()
    {
        PathTrigger.PathTriggerEvent += PlayerPathHandler;
        _onPath = false;
    }

    public override void Exit()
    {
        PathTrigger.PathTriggerEvent -= PlayerPathHandler;
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
        if (!_onPath)
        {
            _xInput = Input.GetAxisRaw("Horizontal");

            if (_canMoveY)
            {
                float yInput = Input.GetAxisRaw("Vertical");

                if (yInput == (int)_path.Direction)
                {
                    _xInput = 0;
                    _onPath = true;
                    PathMove();
                }
            }
        }
        
    }

    void PlayerMovement()
    {
        // Calculate desired velocity
        float targetVelocity = _xInput * _player.MSpeed;

        // Find diff between desired velocity and current velocity
        float velocityDif = targetVelocity - _player.Rb.velocity.x;

        // Check whether to accel or deccel
        float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? _player.MAcceleration :
            _player.MDecceleration;

        // Calc force by multiplying accel and velocity diff, and applying velocity power
        float movement = Mathf.Pow(Mathf.Abs(velocityDif) * accelRate, _player.MVelocityPower)
            * Mathf.Sign(velocityDif);

        _player.Rb.AddForce(movement * Vector3.right);
    }

    void PlayerPathHandler(PathTrigger path, bool canMove)
    {
        _canMoveY = canMove;
        _path = path;
    }

    void PathMove()
    {
        _player.Rb.velocity = Vector3.zero;
        _player.StopAllCoroutines();
        _player.StartCoroutine(IPathMove(_path));
    }


    IEnumerator IPathMove(PathTrigger currentPath)
    {
        _player.transform.position = currentPath.transform.position;

        Debug.Log(currentPath.ConnectedPath.position);
        while (Vector3.Distance(_player.transform.position, currentPath.ConnectedPath.position) > 0.1f)
        {
            _player.transform.position = Vector3.Lerp(_player.transform.position,
                currentPath.ConnectedPath.position, Time.deltaTime * _player.MSpeed);
            yield return null;
        }

        _player.transform.position = currentPath.ConnectedPath.position;
        _onPath = false;
    }
}
