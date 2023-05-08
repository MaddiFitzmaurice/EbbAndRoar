using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class StateMachine
{
    public BaseState CurrentState;

    public StateMachine(BaseState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BaseState newState)
    {
        Assert.IsNotNull(newState);
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
