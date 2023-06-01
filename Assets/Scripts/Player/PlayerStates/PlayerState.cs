using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum FormType { Human, Lion, Leap, UpJump, DownJump }

public class PlayerState : BaseState
{
    public static Action<bool> DirectionChangeEvent;

    protected Player Player;
    bool _canTransform;

    public PlayerState(Player player)
    {
        Player = player;
    }
    
    public override void Enter()
    {
        _canTransform = false;
        MagicPool.MagicPoolEvent += MagicPoolEventHandler;
        Mage.MageEvent += MageEventHandler;
    }

    public override void Exit()
    {
        MagicPool.MagicPoolEvent -= MagicPoolEventHandler;
        Mage.MageEvent -= MageEventHandler;
    }

    public override void LogicUpdate()
    {
        GetXInput();
        TransformForm();
        Player.IsGrounded = GroundCheck();
    }

    public override void PhysicsUpdate()
    {
        PlayerMovement();
    }

    public void PlayerMovement()
    {
        // Calculate desired velocity
        float targetVelocity = Player.XInput * Player.CurrentData.Speed;

        // Find diff between desired velocity and current velocity
        float velocityDif = targetVelocity - Player.Rb.velocity.x;

        // Check whether to accel or deccel
        float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? Player.CurrentData.Acceleration :
            Player.CurrentData.Decceleration;

        // Calc force by multiplying accel and velocity diff, and applying velocity power
        float movement = Mathf.Pow(Mathf.Abs(velocityDif) * accelRate, Player.CurrentData.VelocityPower)
            * Mathf.Sign(velocityDif);

        Player.Rb.AddForce(movement * Vector3.right);
    }

    public void GetXInput()
    {
        Player.XInput = Input.GetAxisRaw("Horizontal");

        // Check facing direction and update what it influences
        // Right facing
        if (Player.XInput == 1)
        {
            Player.IsFacingRight = true;
            Player.transform.right = Vector3.right;
        }
        // Left facing
        else if (Player.XInput == -1)
        {
            Player.IsFacingRight = false;
            Player.transform.right = Vector3.left;
        }

        DirectionChangeEvent?.Invoke(Player.IsFacingRight);
    }

    // Change Player Data, Sprite, and Colliders
    protected void ChangeForms(FormType formType)
    {
        ResetColliders();

        if (formType == FormType.Human)
        {
            Player.IsLion = false;
            Player.CurrentData = Player.HumanData;
            Player.Sprite.sprite = Player.HumanSprite;
            Player.HumanColliders.SetActive(true);
        }
        else if (formType == FormType.Lion)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionSprite;
            Player.LionColliders.SetActive(true);
        }
        else if (formType == FormType.Leap)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionMoveJumpSprite;
            Player.LeapColliders.SetActive(true);
        }
        else if (formType == FormType.UpJump)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionIdleJumpUpSprite;
            Player.HighJumpUpColliders.SetActive(true);
        }
        else if (formType == FormType.DownJump)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionIdleJumpDownSprite;
            Player.HighJumpDownColliders.SetActive(true);
        }
    }

    void ResetColliders()
    {
        Player.HumanColliders.SetActive(false);
        Player.LionColliders.SetActive(false);
        Player.LeapColliders.SetActive(false);
        Player.HighJumpUpColliders.SetActive(false);
        Player.HighJumpDownColliders.SetActive(false);
    }

    protected bool GroundCheck()
    {
        return Physics.BoxCast(Player.LionBodyRefCollider.gameObject.transform.position, Player.GroundCheckCollider.bounds.extents * 2, Vector3.down,
            out RaycastHit hit, Player.transform.rotation, 0.7f, LayerMask.GetMask("Walkable"));
    }

    public void TransformForm()
    {
        // Interact logic
        if (Input.GetKeyDown(KeyCode.E))
        {
            // If can transform
            if (_canTransform)
            {
                // If is not lion
                if (!Player.IsLion)
                {
                    Player.StateMachine.ChangeState(Player.L_IdleState);
                }
                // If is lion
                else 
                {
                    Player.StateMachine.ChangeState(Player.H_MoveState);
                }
            }
        }
    }

    public void MagicPoolEventHandler(bool canTransform)
    {
        _canTransform = canTransform;
    }
    
    void MageEventHandler(bool active)
    {
        Debug.Log("Oh no");
        if (active)
        {
            Player.StateMachine.ChangeState(Player.NarrativeState);
        }
        else if (!active && Player.IsLion)
        {
            Player.StateMachine.ChangeState(Player.L_IdleState);
        }
        else if (!active && !Player.IsLion)
        {
            Player.StateMachine.ChangeState(Player.H_MoveState);
        }
    }
}
