using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitFSM
{
    public UnitState CurrentState { get; private set; }

    //State 변경 후 호출되는 함수
    public void Initialize(UnitState startingState)
    {
        //현재 State 적용
        CurrentState = startingState;
        CurrentState.Enter();
    }

    //State 변경 시 호출되는 함수
    public void ChangeState(UnitState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
