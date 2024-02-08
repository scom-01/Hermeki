using System;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class ActionEffect : ActionData
    {
        [field: SerializeField] public EffectPrefab[] EffectParticles { get; private set; }
    }
}
