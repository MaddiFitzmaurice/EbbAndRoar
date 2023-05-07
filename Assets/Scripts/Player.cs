using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; private set; }
    public Collider Collider { get; private set; }

    // Data
    [Header("Move State Data")]
    public float MSpeed;
    public float MVelocityPower;
    public float MAcceleration;
    public float MDecceleration;

    [Header("Jump State Data")]
    public float JumpHeight;
    public float JSpeed;
    public float JVelocityPower;
    public float JAcceleration;
    public float JDecceleration;

    // Movement
    public float XInput { get; private set; }

    // StateMachine
    public StateMachine StateMachine { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerMoveState MoveState { get; private set; } 
    
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

    public void Movement(float xInput, float speed, float accel, float deccel, float power)
    {
        // Calculate desired velocity
        float targetVelocity = xInput * speed;

        // Find diff between desired velocity and current velocity
        float velocityDif = targetVelocity - Rb.velocity.x;

        // Check whether to accel or deccel
        float accelRate = (Mathf.Abs(targetVelocity) > 0.01f) ? accel :
            deccel;

        // Calc force by multiplying accel and velocity diff, and applying velocity power
        float movement = Mathf.Pow(Mathf.Abs(velocityDif) * accelRate, power)
            * Mathf.Sign(velocityDif);

        Rb.AddForce(movement * Vector3.right);
    }
}
