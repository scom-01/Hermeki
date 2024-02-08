using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKnockBackable
{
    /// <summary>
    /// 캐릭터 넉백
    /// </summary>
    /// <param name="angle">캐릭터가 넉백될 각도</param>
    /// <param name="strength">캐릭터가 넉백될 힘</param>
    /// <param name="direction">캐릭터가 향하는 방향</param>
    void KnockBack(Vector2 angle, float strength, int direction);
}
