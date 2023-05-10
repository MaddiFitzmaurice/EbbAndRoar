using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHuman : Player
{
    public HumanMoveState MoveState;
    
    void Start()
    {
        MoveState = new HumanMoveState(this);
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
