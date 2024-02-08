using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class DamageData : ComponentData<ActionDamage>
    {
        public DamageData()
        {
            ComponentDependency = typeof(Damage);
        }
    }
}
