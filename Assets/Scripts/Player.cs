using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    public Rigidbody Rb;
    public Collider Collider;

    // Data
    public float Speed;
    public float VelocityPower;
    public float Acceleration;
    public float Decceleration;
    public float JumpHeight;

    // Movement
    [HideInInspector] public Vector3 MoveDir;

    // StateMachine
    public StateMachine StateMachine;
    public PlayerJumpState JumpState;
    public PlayerMoveState MoveState; 
    
    // Start is called before the first frame update
    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    void Start()
    {
        MoveState = new PlayerMoveState(this);
        JumpState = new PlayerJumpState(this);
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
