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
    Path _path;

    // Lion Transformation
    bool _canTransform;

    // NPC Talking
    bool _canTalk;

    public HumanMoveState(Player player) : base(player)
    {

    }

    public override void Enter()
    {
        // Event Subscriptions
        Path.PathEvent += PathEventHandler;
        MagicPool.MagicPoolEvent += MagicPoolEventHandler;
        NPC.StartNarrativeEvent += NPCEventHandler;

        // Set Interactable Flags
        _onPath = false;
        _canTransform = false;
        _canTalk = false;

        // Change Data
        Player.CurrentData = Player.HumanData;
        Player.Sprite.sprite = Player.HumanSprite;
        ChangeColliders();
        Player.IsLion = false;
        Player.LionTimer = 0;

        Debug.Log("Human");
    }

    public override void Exit()
    {
        // Event Subscriptions
        Path.PathEvent -= PathEventHandler;
        MagicPool.MagicPoolEvent -= MagicPoolEventHandler; 
        NPC.StartNarrativeEvent -= NPCEventHandler;
    }

    public override void LogicUpdate()
    {
        PlayerInput();

        // Interact logic
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If can transform into Lion
            if (_canTransform)
            {
                _canTransform = false;
                Player.StateMachine.ChangeState(Player.L_MoveState);
            }

            if (_canTalk)
            {
                _canTalk = false;
                Player.StateMachine.ChangeState(Player.H_NarrativeState);
            }
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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PathMove();
                }
            }
        }
    }

    // Interactable Handlers
    void MagicPoolEventHandler(bool canTransform)
    {
        _canTransform = canTransform;
    }

    void PathEventHandler(Path path, bool canMove)
    {
        _canMoveY = canMove;
        _path = path;
    }

    void NPCEventHandler(bool canInteract, Transform transform)
    {
        _canTalk = canInteract;
    }

    void PathMove()
    {
        Player.XInput = 0;
        _onPath = true;
        Player.Rb.velocity = Vector3.zero;
        Player.StopAllCoroutines();
        Player.StartCoroutine(IPathMove(_path));
    }

    IEnumerator IPathMove(Path currentPath)
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

    void ChangeColliders()
    {
        Player.H_Collider.enabled = true;
        Player.L_Collider.enabled = false;
        Player.L_SlipCollider.enabled = false;
    }
}
