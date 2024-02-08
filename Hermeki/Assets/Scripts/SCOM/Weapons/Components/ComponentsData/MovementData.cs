using SCOM.CoreSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class MovementData : ComponentData<ActionMovement>
    {
        public MovementData()
        {
            ComponentDependency = typeof(WeaponMovement);
        }
    }
}
