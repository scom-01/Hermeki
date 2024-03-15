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
        if (CurrentState == newState) 
        {
            Debug.Log($"이전 상태와 동일하여 반환");
            return;
        }
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
