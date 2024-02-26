using System;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    [Serializable]
    public class ActionWeaponBuff : ActionData
    {
        [field: SerializeField] public WeaponComponentBuffData[] BuffDataList { get; private set; }
    }

    [Serializable]
    public struct WeaponComponentBuffData
    {
        /// <summary>
        /// 버프 데이터
        /// </summary>
        public BuffItemSO Buff;
        /// <summary>
        /// 버프효과사운드
        /// </summary>
        public AudioData EquipAudio;
    }
}
