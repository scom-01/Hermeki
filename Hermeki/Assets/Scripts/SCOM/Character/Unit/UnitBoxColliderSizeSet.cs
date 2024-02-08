using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBoxColliderSizeSet : MonoBehaviour
{
    private BoxCollider2D BC2D;
    private Unit Unit;

    void Start()
    {
        Unit = GetComponentInParent<Unit>();
        BC2D = GetComponent<BoxCollider2D>();
        BC2D.isTrigger = true;
        BC2D.offset = Unit.CC2D.offset;
        BC2D.size = Unit.CC2D.size;
    }
}