using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHuman : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; private set; }

    // State Machine
    public StateMachine StateMachine;
    public PlayerHumanMoveState MoveState;

    // Data
    [Header("Move State Data")]
    public float MSpeed;
    public float MVelocityPower;
    public float MAcceleration;
    public float MDecceleration;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        MoveState = new PlayerHumanMoveState(this);
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
