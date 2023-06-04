using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class HumanMoveState : PlayerState
{
    // Path Movement
    bool _onPath;
    bool _canMoveY;
    Path _path;

    // NPC Talking
    bool _canTalk;

    // Mechanism Interaction
    bool _canOperateMech;

    // Falling
    bool _isFalling;

    public static Action OperatedMechEvent;

    public HumanMoveState(Player player) : base(player)
    {

    }
    
    public override void Enter() 
    {   
        base.Enter();
        // Event Subscriptions
        Path.PathEvent += PathEventHandler;
        NPC.SendNarrativeDataEvent += NPCEventHandler;
        Mechanism.MechanismEvent += MechanismEventHandler;

        // Set Interactable Flags
        ResetInteractableFlags();

        ChangeForms(FormType.Human);
        _isFalling = false;
    }

    public override void Exit()
    {
        base.Exit();
        // Event Subscriptions
        Path.PathEvent -= PathEventHandler;
        NPC.SendNarrativeDataEvent -= NPCEventHandler;
        Mechanism.MechanismEvent -= MechanismEventHandler;
    }

    public override void LogicUpdate()
    {
        PlayerInput();
        IsFalling();
        TransformForm();

        // Interact logic
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_canTalk)
            {
                _canTalk = false;
                Player.StateMachine.ChangeState(Player.NarrativeState);
            }

            if (_canOperateMech)
            {
                _canOperateMech = false;
                OperatedMechEvent?.Invoke();
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

    void ResetInteractableFlags()
    {
        _onPath = false;
        _canTalk = false;
        _canOperateMech = false;
    }

    void PathEventHandler(Path path, bool canMove)
    {
        _canMoveY = canMove;
        _path = path;
    }

    void MechanismEventHandler(bool canOperateMech)
    {
        _canOperateMech = canOperateMech;
    }

    void NPCEventHandler(NPCEventData npcEventData)
    {
        _canTalk = npcEventData.CanInteract;
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
        Vector3 startPos = Player.transform.position;
        float elapsedTime = 0;

        if (currentPath.ConnectedPath.position.z < currentPath.transform.position.z)
        {
            ChangeSortingLayer(currentPath);
        }

        while (Vector3.Distance(Player.transform.position, currentPath.ConnectedPath.position) > 0.1f)
        {
            Player.transform.position = Vector3.Lerp(startPos,
                currentPath.ConnectedPath.position, elapsedTime / Player.PathMoveDuration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Player.transform.position = currentPath.ConnectedPath.position;
        _onPath = false;

        if (currentPath.ConnectedPath.position.z > currentPath.transform.position.z)
        {
            ChangeSortingLayer(currentPath);
        }
    }

    void IsFalling()
    {
        if (Player.Rb.velocity.y < -1f && !_isFalling && !GroundCheck())
        {
            _isFalling = true;
            Player.Sprite.sprite = Player.HumanFallingSprite;
        }
        else if (Player.Rb.velocity.y >= -1f && _isFalling && GroundCheck()) 
        {
            _isFalling = false;
            Player.Sprite.sprite = Player.HumanSprite;
        }
    }

    void ChangeSortingLayer(Path currentPath)
    {
        if (currentPath.ConnectedPath.position.z == 0)
        {
            Debug.Log("0");
            Player.Sprite.sortingLayerName = "Z0";
        }
        else if (currentPath.ConnectedPath.position.z == 3)
        {
            Debug.Log("3");
            Player.Sprite.sortingLayerName = "Z3";
        }
        else if (currentPath.ConnectedPath.position.z == 6)
        {
            Player.Sprite.sortingLayerName = "Z6";
        }
    }
}
