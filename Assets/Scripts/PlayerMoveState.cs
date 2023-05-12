using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMoveState : BaseState
{
    public static Action<bool> DirectionChangeEvent;

    protected Player Player;

    public PlayerMoveState(Player player)
    {
        Player = player;
    }

    public override void LogicUpdate()
    {
        GetXInput();

        if (Player.IsLion)
        {
            Player.LionTimer += Time.deltaTime;
        }

        Debug.Log(Player.LionTimer);
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

        // Check facing direction and update camera blend
        if (Player.XInput == 1)
        {
            Player.IsFacingRight = true;
            Player.Sprite.flipX = false;
        }
        else if (Player.XInput == -1)
        {
            Player.IsFacingRight = false;
            Player.Sprite.flipX = true;
        }

        DirectionChangeEvent?.Invoke(Player.IsFacingRight);
    }
}
