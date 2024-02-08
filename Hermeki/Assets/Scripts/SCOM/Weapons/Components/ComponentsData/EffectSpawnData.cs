using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class EffectSpawnData : ComponentData<ActionEffect>
    {
        public EffectSpawnData()
        {
            ComponentDependency = typeof(EffectSpawn);
        }
    }
}
