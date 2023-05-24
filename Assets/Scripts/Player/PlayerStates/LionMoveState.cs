using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LionMoveState : PlayerMoveState
{    
    public LionMoveState(Player player) : base(player)
    {
       
    }

    public override void Enter()
    {
        // Change Data
        Player.CurrentData = Player.LionData;
        Player.Sprite.sprite = Player.LionSprite;
        ChangeColliders();
        Player.IsLion = true;

        Debug.Log("Lion Move State");
    }

    public override void Exit()
    {

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Jump 
        if (Input.GetButtonDown("Jump") && Player.IsGrounded)
        {
            Player.StateMachine.ChangeState(Player.L_JumpState);
        }

        // When Lion Time is up, change back to human
        if (Player.LionTimer >= Player.LionTime)
        {
            Player.StateMachine.ChangeState(Player.H_MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void ChangeColliders()
    {
        Player.H_Collider.enabled = false;
        Player.H_SlipCollider.enabled = false;
        Player.L_Collider.enabled = true;
        Player.L_SlipCollider.enabled = true;
    }
}
