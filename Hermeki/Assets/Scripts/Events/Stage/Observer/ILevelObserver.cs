/// <summary>
/// 레벨 매니저 옵저버, 레벨 매니저의 변경 사항을 받아서 호출됨, Push방식
/// </summary>
public interface ILevelManagerObserver
{
    public virtual void UpdateObserver(int _value) { }
    public virtual void UpdateStageIdx(int _value) { }
    public virtual void UpdateStageLevel(int _value) { }
}
