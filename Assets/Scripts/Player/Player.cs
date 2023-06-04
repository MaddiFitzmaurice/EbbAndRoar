using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; set; }
    public SpriteRenderer Sprite { get; set; }

    [Header("Start Position")]
    [SerializeField] Vector3 _startPos;

    // Colliders
    [Header("Colliders")]
    public BoxCollider GroundCheckCollider;
    public GameObject HumanColliders;
    public GameObject LionColliders;
    public GameObject LeapColliders;
    public GameObject HighJumpUpColliders;
    public GameObject HighJumpDownColliders;
    public Collider LionBodyRefCollider;

    // State Machine
    public StateMachine StateMachine;

    // States
    public LionMoveState L_MoveState;
    public LionMoveJumpState L_MoveJumpState;
    public LionIdleState L_IdleState;
    public LionIdleJumpState L_IdleJumpState;
    public HumanMoveState H_MoveState;
    public PlayerNarrativeState NarrativeState;

    // Movement
    public float XInput { get; set; }
    public bool IsFacingRight { get; set; }
    public bool IsGrounded {get; set; }

    // Sprites
    [Header("Sprites")]
    public Sprite LionSprite;
    public Sprite LionWalkingSprite;
    public Sprite LionMoveJumpSprite;
    public Sprite LionIdleJumpUpSprite;
    public Sprite LionIdleJumpDownSprite;
    public Sprite HumanSprite;
    public Sprite HumanFallingSprite;

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
    public float IdleJumpHeight;
    public float MoveJumpHeight;

    [Header("Gravity Modifier")]
    public Vector3 GravityNorm;
    public Vector3 GravityUp;
    public Vector3 GravityDown;

    [HideInInspector] public bool IsLion;

    // Data Objects
    public PlayerData HumanData { get; set; }
    public PlayerData LionData { get; set; }
    public PlayerData CurrentData { get; set; }

    // Ground check testing
    public RaycastHit hit { get; set; }
    
    void Awake()
    {
        Rb = GetComponent<Rigidbody>();
        Sprite = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        Cursor.visible = false;
        transform.position = _startPos;
        Sprite.sortingLayerName = "Z0";
        DataSetup();
        StateMachineSetup();
    }

    void StateMachineSetup()
    {
        L_MoveState = new LionMoveState(this);
        L_MoveJumpState = new LionMoveJumpState(this);
        L_IdleState = new LionIdleState(this);
        L_IdleJumpState = new LionIdleJumpState(this);
        H_MoveState = new HumanMoveState(this);
        NarrativeState = new PlayerNarrativeState(this);
        StateMachine = new StateMachine(L_IdleState);
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
    /*
    public void OnDrawGizmos()
    {
        //Check if there has been a hit yet
        if (IsGrounded)
        {
            //Debug.Log("IsGrounded");
            Gizmos.color = Color.blue;
            //Draw a Ray forward from GameObject toward the hit
            Gizmos.DrawRay(LionBodyRefCollider.gameObject.transform.position, Vector3.down * hit.distance);
            //Draw a cube that extends to where the hit exists
            Gizmos.DrawWireCube(LionBodyRefCollider.gameObject.transform.position + Vector3.down * hit.distance, GroundCheckCollider.bounds.extents * 2);
        }
        //If there hasn't been a hit yet, draw the ray at the maximum distance
        else
        {
            //Debug.Log("Is in air");
            Gizmos.color = Color.red;
            //Draw a Ray forward from GameObject toward the maximum distance
            Gizmos.DrawRay(LionBodyRefCollider.gameObject.transform.position, Vector3.down * 0.7f);
            //Draw a cube at the maximum distance
            Gizmos.DrawWireCube(LionBodyRefCollider.gameObject.transform.position + Vector3.down * 0.7f, GroundCheckCollider.bounds.extents * 2);
        }
    }
    */
}
