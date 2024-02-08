using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{ 
    public class WeaponSpriteData : ComponentData<ActionSprites>
    {   
        public WeaponSpriteData()
        {
            ComponentDependency = typeof(WeaponSprite);
        }
    }
}
