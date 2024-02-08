using SCOM.Weapons.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class ShakeCamData : ComponentData<ActionShakeCam>
    {
        public ShakeCamData()
        {
            ComponentDependency = typeof(ShakeCam);
        }
    }
}
