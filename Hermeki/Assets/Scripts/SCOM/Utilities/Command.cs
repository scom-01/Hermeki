using System;

public class Command
{
    private event Action command
    {
        add
        {
            _command -= value;
            _command += value;
        }
        remove => _command -= value;
    }
    private event Action _command;

    /// <summary>
    /// 중복을 제거하면서 함수 추가
    /// </summary>
    /// <param name="action"></param>
    public void Add(Action action) => command += action;

    /// <summary>
    /// 함수 제거
    /// </summary>
    /// <param name="action"></param>
    public void Remove(Action action) => command -= action;

    public bool Excute()
    {
        if (_command == null)
            return false;

        _command?.Invoke();        
        return true;
    }

    public void Clear() => _command = null;
}
