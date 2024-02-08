using SCOM.Weapons.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class WeaponProjectileData : ComponentData<WeaponProjectileActionData>
    {

        public WeaponProjectileData()
        {
            ComponentDependency = typeof(WeaponProjectile);
        }
    }
}
