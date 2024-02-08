using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class ActionKnockback : ActionData
    {
        [field: SerializeField] public Vector2[] KnockbackAngle { get; private set; }
    }
}
