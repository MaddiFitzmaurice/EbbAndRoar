using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; set; }
    public Collider Collider { get; set; }
    public SpriteRenderer Sprite { get; set; }

    // State Machine
    public StateMachine StateMachine;
    

    // Movement
    public float XInput { get; set; }
    public bool IsFacingRight { get; set; }

    // Data
    [Header("Human State Data")]
    public float HSpeed;
    public float HVelocityPower;
    public float HAcceleration;
    public float HDecceleration;
    
    // Data
    [Header("Lion State Data")]
    public float LSpeed;
    public float LVelocityPower;
    public float LAcceleration;
    public float LDecceleration;
    public float JumpHeight;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // State Machine 
    public LionJumpState JumpState { get; private set; }
    public LionMoveState MoveState { get; private set; } 

    void Start()
    {
        //HumanState = new HumanState(this);
        //LionState = new LionState(this);
        StateMachine = new StateMachine(MoveState);
    }
    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
}
