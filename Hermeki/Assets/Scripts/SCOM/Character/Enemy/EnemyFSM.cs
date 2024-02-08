using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM
{
    public EnemyState CurrentState { get; private set; }

    //State ���� �� ȣ��Ǵ� �Լ�
    public void Initialize(EnemyState startingState)
    {
        //���� State ����
        CurrentState = startingState;
        CurrentState.Enter();
    }

    //State ���� �� ȣ��Ǵ� �Լ�
    public void ChangeState(EnemyState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
