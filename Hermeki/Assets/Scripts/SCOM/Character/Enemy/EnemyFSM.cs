using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM
{
    public EnemyState CurrentState { get; private set; }

    //State 변경 후 호출되는 함수
    public void Initialize(EnemyState startingState)
    {
        //현재 State 적용
        CurrentState = startingState;
        CurrentState.Enter();
    }

    //State 변경 시 호출되는 함수
    public void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
