using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit_Aniamation_Teleport : MonoBehaviour
{
    private Unit unit
    {
        get
        {
            return this.GetComponentInParent<Unit>();
        }
    }

    private Unit TargetUnit;

    public void SetTarget(Unit TargetUnit)
    {
        this.TargetUnit = TargetUnit;
    }
    public void Teleport()
    {
        if (TargetUnit == null)
            return;

        unit.transform.position = unit.GetTarget().Core.CoreCollisionSenses.UnitCenterPos;
        Debug.Log($"{unit.name} to {TargetUnit.name} Teleport");
    }
}
