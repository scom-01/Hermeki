using System;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class ActionSoundClip : ActionData
    {
        [field: SerializeField] public AudioPrefab[] audioDataList;
    }
}
