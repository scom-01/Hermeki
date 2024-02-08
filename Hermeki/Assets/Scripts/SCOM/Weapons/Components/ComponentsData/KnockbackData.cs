using SCOM.Weapons.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class KnockbackData : ComponentData<ActionKnockback>
    {
        public KnockbackData()
        {
            ComponentDependency = typeof(Knockback);
        }
    }
}
