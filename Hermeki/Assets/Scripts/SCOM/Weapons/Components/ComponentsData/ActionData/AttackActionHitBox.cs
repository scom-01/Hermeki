using System;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class AttackActionHitBox : ActionData
    {
        [field: SerializeField] public HitAction[] ActionHit;
    }
}
