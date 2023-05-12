using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class HumanMoveState : PlayerMoveState
{
    // Path Movement
    bool _onPath;
    bool _canMoveY;
    PathTrigger _path;

    public HumanMoveState(Player player) : base(player)
    {

    }

    public override void Enter()
    {
        PathTrigger.PathTriggerEvent += PlayerPathHandler;
        _onPath = false;
        Player.CurrentData = Player.HumanData;
        Player.IsLion = false;
        Debug.Log("Human");
    }

    public override void Exit()
    {
        PathTrigger.PathTriggerEvent -= PlayerPathHandler;
    }

    public override void LogicUpdate()
    {
        PlayerInput();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.StateMachine.ChangeState(Player.L_MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void PlayerInput()
    {
        if (!_onPath)
        {
            GetXInput();

            if (_canMoveY)
            {
                float yInput = Input.GetAxisRaw("Vertical");

                if (yInput == (int)_path.Direction)
                {
                    PathMove();
                }
            }
        }
        
    }

    void PlayerPathHandler(PathTrigger path, bool canMove)
    {
        _canMoveY = canMove;
        _path = path;
    }

    void PathMove()
    {
        Player.XInput = 0;
        _onPath = true;
        Player.Rb.velocity = Vector3.zero;
        Player.StopAllCoroutines();
        Player.StartCoroutine(IPathMove(_path));
    }

    IEnumerator IPathMove(PathTrigger currentPath)
    {
        Player.transform.position = currentPath.transform.position;

        while (Vector3.Distance(Player.transform.position, currentPath.ConnectedPath.position) > 0.1f)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position,
                currentPath.ConnectedPath.position, Time.deltaTime * Player.H_Speed);
            yield return null;
        }

        Player.transform.position = currentPath.ConnectedPath.position;
        _onPath = false;
    }
}
