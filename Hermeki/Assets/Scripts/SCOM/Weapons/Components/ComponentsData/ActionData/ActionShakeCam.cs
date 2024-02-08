using System;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class ActionShakeCam : ActionData
    {
        [field: SerializeField] public CamData[] ShakeCamData { get; private set; }
    }
    [Serializable]
    public struct CamData
    {
        [Range(0.1f, 20f)] public float Duration;
        [Range(0.01f, 0.1f)] public float Range;
        [Range(0.001f, 0.01f)] public float RepeatRate;
    }
}
