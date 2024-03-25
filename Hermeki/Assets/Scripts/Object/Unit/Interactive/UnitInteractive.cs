using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitInteractive : MonoBehaviour
{
    public bool isDuration = false;
    public float DurationTime;
    public float ElapsedTime;

    public virtual void Init()
    {
        isDuration = false;
        ElapsedTime = 0f;
    }
    public virtual void Interactive(Unit unit)
    {

    }
    public virtual void UnInteractive(Unit unit)
    {

    }

    public virtual void LogicUpdate(Unit unit)
    {

    }
}
