using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; set; }
    public Collider Collider { get; set; }

    // State Machine
    public StateMachine StateMachine;

    // Movement
    public float XInput { get; set; }
    public bool IsFacingRight { get; set; }

    // Data
    [Header("Move State Data")]
    public float Speed;
    public float VelocityPower;
    public float Acceleration;
    public float Decceleration;
}
