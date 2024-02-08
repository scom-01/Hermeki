using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class ActionHitBoxData : ComponentData<AttackActionHitBox>
    {
        [field: SerializeField] public LayerMask DetectableLayers { get; private set; }

        public ActionHitBoxData()
        {
            ComponentDependency = typeof(ActionHitBox);
        }
    }
}
