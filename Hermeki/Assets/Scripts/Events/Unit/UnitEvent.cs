using UnityEngine;

public class UnitEvent : MonoBehaviour
{
    public Unit unit;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        if(unit != null)
            unit.Core.CoreUnitStats.OnHealthZero += Action;
    }

    public virtual void Action() { }
}
