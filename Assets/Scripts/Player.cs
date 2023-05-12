using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; set; }
    public Collider Collider { get; set; }
    public SpriteRenderer Sprite { get; set; }

    // State Machine
    public StateMachine StateMachine;

    // States
    public LionMoveState L_MoveState;
    public LionJumpState L_JumpState;
    public HumanMoveState H_MoveState;

    // Movement
    public float XInput { get; set; }
    public bool IsFacingRight { get; set; }

    // Data
    [Header("Human State")]
    public float H_Speed;
    public float H_VelocityPower;
    public float H_Acceleration;
    public float H_Decceleration;

    [Header("Lion State")]
    public float L_Speed;
    public float L_VelocityPower;
    public float L_Acceleration;
    public float L_Decceleration;
    public float JumpHeight;
    public float LionTime;

    // Data Objects
    public PlayerData HumanData;
    public PlayerData LionData;
    public PlayerData CurrentData;

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        DataSetup();
        StateMachineSetup();
    }

    void StateMachineSetup()
    {
        L_MoveState = new LionMoveState(this);
        L_JumpState = new LionJumpState(this);
        H_MoveState = new HumanMoveState(this);
        StateMachine = new StateMachine(H_MoveState);
    }

    void DataSetup()
    {
        HumanData = new PlayerData(H_Speed, H_VelocityPower, H_Acceleration, H_Decceleration);
        LionData = new PlayerData(L_Speed, L_VelocityPower, L_Acceleration, L_Decceleration);
        CurrentData = HumanData;
    }

    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
}
