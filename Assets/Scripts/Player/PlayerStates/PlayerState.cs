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
    
    Collider[] _humanColliders;
    Collider[] _lionColliders;
    Collider[] _leapColliders;
    Collider[] _jumpUpColliders;
    Collider[] _jumpDownColliders;

    public PlayerState(Player player)
    {
        Player = player;
        SetUpColliders();
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
            ToggleFormColliders(_humanColliders, true);
        }
        else if (formType == FormType.Lion)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionSprite;
            ToggleFormColliders(_lionColliders, true);
        }
        else if (formType == FormType.Leap)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionMoveJumpSprite;
            ToggleFormColliders(_leapColliders, true);
        }
        else if (formType == FormType.UpJump)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionIdleJumpUpSprite;
            ToggleFormColliders(_jumpUpColliders, true);
        }
        else if (formType == FormType.DownJump)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionIdleJumpDownSprite;
            ToggleFormColliders(_jumpDownColliders, true);
        }
    }

    void SetUpColliders()
    {
        _humanColliders = Player.HumanColliders.GetComponentsInChildren<Collider>();
        _lionColliders = Player.LionColliders.GetComponentsInChildren<Collider>();
        _leapColliders = Player.LeapColliders.GetComponentsInChildren<Collider>();
        _jumpUpColliders = Player.HighJumpUpColliders.GetComponentsInChildren<Collider>();
        _jumpDownColliders = Player.HighJumpDownColliders.GetComponentsInChildren<Collider>();
    }

    void ResetColliders()
    {
        ToggleFormColliders(_humanColliders, false);
        ToggleFormColliders(_lionColliders, false);
        ToggleFormColliders(_leapColliders, false);
        ToggleFormColliders(_jumpUpColliders, false);
        ToggleFormColliders(_jumpDownColliders, false);
    }

    void ToggleFormColliders(Collider[] colliders, bool toggle)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = toggle;
        }
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
        // If mage event started, change to narrative state
        if (active)
        {
            Player.StateMachine.ChangeState(Player.NarrativeState);
        }
        // Else, change back to which ever form player was last in
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
