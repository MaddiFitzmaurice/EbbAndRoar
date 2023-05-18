using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; set; }
    public SpriteRenderer Sprite { get; set; }

    // Colliders
    public BoxCollider L_Collider;
    public BoxCollider L_SlipCollider;
    public CapsuleCollider H_Collider;
    public BoxCollider GroundCheckCollider;

    // State Machine
    public StateMachine StateMachine;

    // States
    public LionMoveState L_MoveState;
    public LionJumpState L_JumpState;
    public HumanMoveState H_MoveState;

    // Movement
    public float XInput { get; set; }
    public bool IsFacingRight { get; set; }
    public bool IsGrounded {get; set; }

    // Sprites
    public Sprite LionSprite;
    public Sprite HumanSprite;

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

    [Header("Gravity Modifier")]
    public Vector3 GravityNorm;
    public Vector3 GravityUp;
    public Vector3 GravityDown;

    // Lion Timer
    [HideInInspector] public float LionTimer;
    [HideInInspector] public bool IsLion;

    // Data Objects
    public PlayerData HumanData { get; set; }
    public PlayerData LionData { get; set; }
    public PlayerData CurrentData { get; set; }

    // Ground check testing
    
    public RaycastHit hit;
    

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
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

    // Jump groundcheck testing
    /*public void OnDrawGizmos()
    {
        //Check if there has been a hit yet
        if (IsGrounded)
        {
            //Debug.Log("IsGrounded");
            Gizmos.color = Color.blue;
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(transform.position, Vector3.down * hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(transform.position + Vector3.down * hit.distance, GroundCheckCollider.bounds.extents * 2);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Debug.Log("Is in air");
            Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(transform.position, Vector3.down * 1f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(transform.position + Vector3.down * 1f, GroundCheckCollider.bounds.extents * 2);
        }
    }*/
}
