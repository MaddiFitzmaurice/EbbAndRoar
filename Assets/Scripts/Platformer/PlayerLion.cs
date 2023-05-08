using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLion : Player
{
    [Header("Jump State Data")]
    public float JumpHeight;

    // State Machine 
    public LionJumpState JumpState { get; private set; }
    public LionMoveState MoveState { get; private set; } 
    
    // Start is called before the first frame update
    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    void Start()
    {
        MoveState = new LionMoveState(this);
        JumpState = new LionJumpState(this);
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
