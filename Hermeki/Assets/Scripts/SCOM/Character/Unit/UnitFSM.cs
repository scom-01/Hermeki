public class UnitFSM
{
    public UnitState CurrentState { get; private set; }

    /// <summary>
    /// 초기 State 설정 함수
    /// </summary>
    /// <param name="startingState"></param>
    public void Initialize(UnitState startingState)
    {
        //현재 State 적용
        CurrentState = startingState;
        CurrentState.Enter();
    }

    /// <summary>
    /// State 변경 시 호출되는 함수
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(UnitState newState)
    {
        //if (CurrentState == newState) 
        //{
        //    Debug.Log($"이전 상태와 동일하여 반환");
        //    return;
        //}
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    /// <summary>
    /// CurrentState가 T와 타입이 같은 지 확인, 상속받는 관계여도 true 반환
    /// </summary>
    /// <typeparam name="T">제약조건 : UnitState</typeparam>
    /// <returns></returns>
    public bool CompareRootState<T>() => CurrentState is T || CurrentState.GetType().IsSubclassOf(typeof(T));

    /// <summary>
    /// CurrentState가 T와 타입이 같은 지 확인, 상속받는 관계일 땐 false 반환
    /// </summary>
    /// <typeparam name="T">제약조건 : UnitState</typeparam>
    /// <returns></returns>
    public bool CompareState<T>() where T : UnitState => CurrentState is T;
}
