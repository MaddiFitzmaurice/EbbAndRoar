using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerState : BaseState
{
    public static Action<bool> DirectionChangeEvent;

    protected Player Player;

    public PlayerState(Player player)
    {
        Player = player;
    }

    public override void LogicUpdate()
    {
        GetXInput();
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
    protected void ChangeToLion(bool isLion)
    {
        foreach (Collider collider in Player.L_Colliders)
        {
            collider.enabled = isLion;
        }

        foreach (Collider collider in Player.H_Colliders)
        {
            collider.enabled = !isLion;
        }

        if (isLion)
        {
            Player.IsLion = true;
            Player.CurrentData = Player.LionData;
            Player.Sprite.sprite = Player.LionSprite;
        }
        else 
        {
            Player.IsLion = false;
            Player.CurrentData = Player.HumanData;
            Player.Sprite.sprite = Player.HumanSprite;
        }
    }

    protected bool GroundCheck()
    {
        return Physics.BoxCast(Player.L_Colliders[1].gameObject.transform.position, Player.GroundCheckCollider.bounds.extents * 2, Vector3.down,
            out RaycastHit hit, Player.transform.rotation, 0.7f, LayerMask.GetMask("Walkable"));
    }
}
