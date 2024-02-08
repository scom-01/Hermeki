using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM 
{
    public PlayerState CurrentState { get; private set; }
        
    //State ���� �� ȣ��Ǵ� �Լ�
    public void Initialize(PlayerState startingState)
    {
        //���� State ����
        CurrentState = startingState;
        CurrentState.Enter();
    }

    //State ���� �� ȣ��Ǵ� �Լ�
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
