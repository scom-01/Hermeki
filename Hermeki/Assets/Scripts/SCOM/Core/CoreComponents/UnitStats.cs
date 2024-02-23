using System;
using UnityEngine;

namespace SCOM.CoreSystem
{
    public class UnitStats : CoreComponent
    {
        public event Action OnHealthZero;
        public event Action OnChangeHealth;
        public event Action OnChangeStats;

        #region Stats       

        public StatsData StatsData { get => Default_statsData; set => Default_statsData = value; }
        [field: Header("Stats")]
        [field: SerializeField] private StatsData Default_statsData;
        public StatsData CalculStatsData
        {
            get
            {
                StatsData data = new StatsData()
                {
                    MaxHealth = Cal_MaxHealth,
                    DefaultPower = Cal_DefaultPower,
                    DefaultMoveSpeed = Cal_DefaultMoveSpeed,
                    DefaultJumpVelocity = Cal_DefaultJumpVelocity,
                    DefaultAttSpeed = Cal_DefaultAttSpeed,
                    MovementVEL_Per = Cal_MoveSpeed,
                    JumpVEL_Per = Cal_JumpVelocity,
                    AttackSpeedPer = Cal_AttackSpeedPer,
                    PhysicsDefensivePer = Cal_PhysicsDefensivePer,
                    MagicDefensivePer = Cal_MagicDefensivePer,
                    PhysicsAggressivePer = Cal_PhysicsAggressivePer,
                    MagicAggressivePer = Cal_MagicAggressivePer,
                    CriticalPer = Cal_CriticalPer,
                    CriticalDmgPer = Cal_CriticalDmgPer,
                    DamageAttiribute = Cal_DamageAttiribute,
                    Elemental = Cal_Elemental,
                    ElementalDefensivePer = Cal_ElementalDefensivePer,
                    ElementalAggressivePer = Cal_ElementalAggressivePer,
                };
                return data;
            }
            set => m_statsData = value;
        }
        /// <summary>
        /// Item, Buff 등 증가로 인한 스탯
        /// </summary>
        public StatsData m_statsData;

        public BlessStatsData BlessStats;

        public float invincibleTime;
        public float TouchinvincibleTime;
        public float CurrentHealth
        {
            get
            {
                if (currentHealth > (Default_statsData.MaxHealth + m_statsData.MaxHealth))
                {
                    currentHealth = Default_statsData.MaxHealth + m_statsData.MaxHealth;
                }
                return currentHealth;
            }
            private set
            {
                currentHealth = value <= 0 ? 0 : (value >= Default_statsData.MaxHealth + m_statsData.MaxHealth ? Default_statsData.MaxHealth + m_statsData.MaxHealth : value);
                OnChangeHealth?.Invoke();
            }
        }
        [SerializeField] private float currentHealth;


        /// <summary>
        /// 물리 방어력 %
        /// </summary>
        private float Cal_PhysicsDefensivePer { get => (Default_statsData.PhysicsDefensivePer + m_statsData.PhysicsDefensivePer + BlessStats.Bless_Def_Lv * GlobalValue.BlessingStats_Inflation); }

        /// <summary>
        /// 마법 방어력 %
        /// </summary>
        private float Cal_MagicDefensivePer { get => (Default_statsData.MagicDefensivePer + m_statsData.MagicDefensivePer + BlessStats.Bless_Def_Lv * GlobalValue.BlessingStats_Inflation); }

        /// <summary>
        /// 공격력
        /// </summary>
        private float Cal_DefaultPower { get => Default_statsData.DefaultPower + m_statsData.DefaultPower; }

        /// <summary>
        /// 추가 물리공격력 %
        /// </summary>
        private float Cal_PhysicsAggressivePer { get => (Default_statsData.PhysicsAggressivePer + m_statsData.PhysicsAggressivePer + BlessStats.Bless_Agg_Lv * GlobalValue.BlessingStats_Inflation); }

        /// <summary>
        /// 추가 마법공격력 %
        /// </summary>
        private float Cal_MagicAggressivePer { get => (Default_statsData.MagicAggressivePer + m_statsData.MagicAggressivePer + BlessStats.Bless_Agg_Lv * GlobalValue.BlessingStats_Inflation); }

        /// <summary>
        /// 크리티컬 확률
        /// </summary>
        private float Cal_CriticalPer { get => Mathf.Clamp((Default_statsData.CriticalPer + m_statsData.CriticalPer + BlessStats.Bless_Critical_Lv * GlobalValue.BlessingStats_Inflation), 0, 100.0f); }

        /// <summary>
        /// 추가 크리티컬 데미지
        /// </summary>
        private float Cal_CriticalDmgPer { get => (Default_statsData.CriticalDmgPer + m_statsData.CriticalDmgPer + BlessStats.Bless_Critical_Lv * GlobalValue.BlessingStats_Inflation); }

        /// <summary>
        /// 원소 속성 (공격과 방어 모두에 적용)
        /// </summary>
        private E_Power Cal_Elemental { get => Default_statsData.Elemental; }

        /// <summary>
        /// 원소 저항력 %
        /// </summary>
        private float Cal_ElementalDefensivePer { get => (Default_statsData.ElementalDefensivePer + m_statsData.ElementalDefensivePer + BlessStats.Bless_Elemental_Lv * GlobalValue.BlessingStats_Inflation); }

        /// <summary>
        /// 원소 공격력 %
        /// </summary>
        private float Cal_ElementalAggressivePer { get => (Default_statsData.ElementalAggressivePer + m_statsData.ElementalAggressivePer + BlessStats.Bless_Elemental_Lv * GlobalValue.BlessingStats_Inflation); }
        /// <summary>
        /// 기본 공격 속도
        /// </summary>
        private float Cal_DefaultAttSpeed { get => Default_statsData.DefaultAttSpeed + m_statsData.DefaultAttSpeed; }
        /// <summary>
        /// 공격속도 (수치만큼 %로 증가)
        /// </summary>
        private float Cal_AttackSpeedPer { get => (Default_statsData.AttackSpeedPer + m_statsData.AttackSpeedPer + BlessStats.Bless_Speed_Lv * GlobalValue.BlessingStats_Inflation); }
        private float Cal_MaxHealth { get => Default_statsData.MaxHealth + m_statsData.MaxHealth; }
        /// <summary>
        /// 기본 이동속도
        /// </summary>
        private float Cal_DefaultMoveSpeed { get => Default_statsData.DefaultMoveSpeed + m_statsData.DefaultMoveSpeed; }
        /// <summary>
        /// 추가 이동속도(수치만큼 %로 증가)
        /// </summary>
        private float Cal_MoveSpeed { get => (Default_statsData.MovementVEL_Per + m_statsData.MovementVEL_Per + BlessStats.Bless_Speed_Lv * GlobalValue.BlessingStats_Inflation); }
        /// <summary>
        /// 기본 점프력
        /// </summary>
        private float Cal_DefaultJumpVelocity { get => Default_statsData.DefaultJumpVelocity + m_statsData.DefaultJumpVelocity; }
        /// <summary>
        /// 추가 점프력 (수치만큼 %로 증가)
        /// </summary>
        private float Cal_JumpVelocity { get => Default_statsData.JumpVEL_Per + m_statsData.JumpVEL_Per; }

        /// <summary>
        /// 공격 속성 
        /// </summary>
        private DAMAGE_ATT Cal_DamageAttiribute { get => Default_statsData.DamageAttiribute; set => Default_statsData.DamageAttiribute = value; }
        #endregion
        private bool isSetup = false;

        protected override void Awake()
        {
            base.Awake();
            isSetup = false;
            if (core.Unit.UnitData != null && !isSetup)
            {
                StatsData = core.Unit.UnitData.statsStats;
                invincibleTime = core.Unit.UnitData.invincibleTime;
                TouchinvincibleTime = core.Unit.UnitData.touchDamageinvincibleTime;
                CurrentHealth = Cal_MaxHealth;
            }
        }

        public void SetHealth(float amount)
        {
            CurrentHealth = amount;
        }

        public float IncreaseHealth(float amount)
        {
            var oldHealth = CurrentHealth;
            CurrentHealth += amount;
            if (amount > 0)
            {
                core.Unit.Inventory?.ItemExeOnHealing(core.Unit, core.Unit.GetTarget());
                core.CoreDamageReceiver.HUD_DmgTxt(1.0f, CurrentHealth - oldHealth, 30, DAMAGE_ATT.Heal, false);
            }

            //증가한 체력량
            return CurrentHealth - oldHealth;
        }

        /// <summary>
        /// 공격하는 주체가 명확하지 않은 경우(낙사, 트랩 등)
        /// </summary>
        /// <param name="elemental"></param>
        /// <param name="attiribute"></param>
        /// <param name="amount"></param>
        public float DecreaseHealth(E_Power elemental, DAMAGE_ATT attiribute, float amount)
        {
            return DecreaseHealth(null, elemental, attiribute, amount);
        }
        public float DecreaseHealth(Unit attacker, DAMAGE_ATT attiribute, float amount)
        {
            float amount1 = CalculateDamageAtt(attacker, attiribute, amount);
            if (amount1 > 0)
            {
                return DecreaseHealth(attacker, amount1);
            }
            return 0f;
        }
        public float DecreaseHealth(Unit attacker, E_Power _elemental, DAMAGE_ATT attiribute, float amount)
        {
            float amount1 = CalculateElementDamage(attacker, _elemental, amount);
            float amount2 = CalculateDamageAtt(attacker, attiribute, amount1);
            if (amount2 > 0)
            {
                return DecreaseHealth(attacker, amount2);
            }
            return 0f;
        }

        public float DecreaseHealth(Unit attacker, float amount)
        {
            var result = DecreaseHealth(amount);
            if (result > 0)
            {
                if (CurrentHealth == 0)
                {
                    attacker?.Inventory?.ItemExeOnKilled(attacker, core.Unit);
                    attacker?.SetTarget(null);
                }
                core.Unit.Inventory?.ItemExeOnDamaged(core.Unit, attacker);
            }
            return result;
        }
        public float DecreaseHealth(float amount)
        {
            core.Unit.HitEffect();
            CurrentHealth -= amount;

            Debug.Log($"{core.transform.parent.name} Health = {currentHealth}");
            if (CurrentHealth == 0.0f)
            {
                OnHealthZero?.Invoke();
            }
            return amount;
        }

        public float CalculateDamage(Unit attacker, float amount)
        {
            #region 원소속성 계산
            float amount1 = CalculateElementDamage(attacker, attacker.Core.CoreUnitStats.CalculStatsData.Elemental, amount);
            #endregion

            #region 속성 계산
            float amount2 = CalculateDamageAtt(attacker, attacker.Core.CoreUnitStats.CalculStatsData.DamageAttiribute, amount1);
            #endregion

            return amount2;
        }

        public float CalculateElementDamage(Unit attacker, E_Power e_Power, float amount)
        {
            if (attacker == null)
                return amount;

            Debug.Log($"Before Calculator ElementalPower = {amount}");

            //Normal이 아닌 속성을 보유하고있을 때
            if (e_Power != E_Power.Normal)
            {
                amount *= (1.0f + attacker.Core.CoreUnitStats.CalculStatsData.ElementalAggressivePer / 100f);
            }
            //Water(4) > Earth(3) > Wind(2) > Fire(1) > Water
            //ex) DefensivePer = 400% => dmg /= (1+ (400f / 100f)) /= 5, 10(dmg) -> 2(dmg)
            if ((int)attacker.Core.CoreUnitStats.CalculStatsData.Elemental == (int)e_Power)
            {
                Debug.Log($"ElementalPower is the  same {CalculStatsData.Elemental}! Not Increase and Not Decrease");
            }
            else
            {
                if ((int)attacker.Core.CoreUnitStats.CalculStatsData.Elemental > (int)e_Power)
                {
                    //ex) DefensivePer = 400% => dmg /= (1+ (400f / 100f - GlobalValue.E_WeakPer(30f)) /= 6.7142.., 10(dmg) -> 1.48..(dmg)
                    if ((int)attacker.Core.CoreUnitStats.CalculStatsData.Elemental == 4 && (int)e_Power == 1)
                    {
                        amount /= (1.0f + (CalculStatsData.ElementalDefensivePer / (100f - (GlobalValue.E_WeakPer * 100f))));
                    }
                    else
                    {
                        amount /= (1.0f + (CalculStatsData.ElementalDefensivePer / (100f + (GlobalValue.E_WeakPer * 100f))));
                    }
                }
                else if ((int)attacker.Core.CoreUnitStats.CalculStatsData.Elemental < (int)e_Power)
                {
                    if ((int)attacker.Core.CoreUnitStats.CalculStatsData.Elemental == 1 && (int)e_Power == 4)
                    {
                        amount /= (1.0f + (CalculStatsData.ElementalDefensivePer / (100f + (GlobalValue.E_WeakPer * 100f))));
                    }
                    else
                    {
                        amount /= (1.0f + (CalculStatsData.ElementalDefensivePer / (100f - (GlobalValue.E_WeakPer * 100f))));
                    }
                }
                //elemental == MyElemental 같거나 Normal일때
                else
                {
                    Debug.Log($"{core.transform.parent.name}의 MyElemental 과 받는 ElememtalPower가 같음! Elemental 증가 및 감소 없음");
                }
            }
            Debug.Log($"After Calculator ElementalPower = {amount}");
            return amount;
        }

        public float CalculateElementDamage(E_Power e_Power, float amount)
        {
            Debug.Log($"Before Calculator ElementalPower = {amount}");

            //Water(4) > Earth(3) > Wind(2) > Fire(1) > Water
            if ((int)e_Power == (int)Cal_Elemental)
            {
                Debug.Log($"ElementalPower is the  same {e_Power}! Not Increase and Not Decrease");
            }
            else
            {
                if ((int)e_Power > (int)Cal_Elemental)
                {
                    if ((int)e_Power == 4 && (int)e_Power == 1)
                    {
                        amount *= (1.0f - GlobalValue.E_WeakPer);
                    }
                    else
                    {
                        amount *= (1.0f + GlobalValue.E_WeakPer);
                    }
                }
                else if ((int)e_Power < (int)e_Power)
                {
                    if ((int)e_Power == 1 && (int)e_Power == 4)
                    {
                        amount *= (1.0f - GlobalValue.E_WeakPer);
                    }
                    else
                    {
                        amount *= (1.0f + GlobalValue.E_WeakPer);
                    }
                }
                //elemental == MyElemental 같거나 Normal일때
                else
                {
                    Debug.Log($"{core.transform.parent.name}의 MyElemental 과 받는 ElememtalPower가 같음! Elemental 증가 및 감소 없음");
                }
            }
            Debug.Log($"After Calculator ElementalPower = {amount}");
            return amount;
        }

        /// <summary>
        /// 공격 속성 계산
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="Damage_att">attcker의 공격속성</param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public float CalculateDamageAtt(Unit attacker, DAMAGE_ATT Damage_att, float amount)
        {
            if (attacker == null)
                return amount;

            Debug.Log($"Before Calculator DamageAttribute = {amount}");
            switch (Damage_att)
            {
                case DAMAGE_ATT.Physics:
                    amount *= (1.0f + attacker.Core.CoreUnitStats.CalculStatsData.PhysicsAggressivePer / 100);
                    amount /= (1.0f + (CalculStatsData.PhysicsDefensivePer / 100f));
                    if (amount <= 0.0f)
                        return 0;
                    break;
                case DAMAGE_ATT.Magic:
                    amount *= (1.0f + attacker.Core.CoreUnitStats.CalculStatsData.MagicAggressivePer / 100);
                    amount /= (1.0f + (CalculStatsData.MagicDefensivePer / 100f));
                    if (amount <= 0.0f)
                        return 0;
                    break;
                case DAMAGE_ATT.Fixed:
                    //고정 데미지 일 시 감소 없음
                    break;
            }
            Debug.Log($"Atfer Calculator DamageAttribute = {amount}");
            return amount;
        }
        public float CalculateDamageAtt(DAMAGE_ATT Damage_att, float amount)
        {
            Debug.Log($"Before Calculator DamageAttribute = {amount}");
            switch (Damage_att)
            {
                case DAMAGE_ATT.Physics:
                    amount /= (1.0f + (CalculStatsData.PhysicsDefensivePer / 100));
                    if (amount <= 0.0f)
                        return 0;
                    break;
                case DAMAGE_ATT.Magic:
                    amount /= (1.0f + (CalculStatsData.MagicDefensivePer / 100));
                    if (amount <= 0.0f)
                        return 0;
                    break;
                case DAMAGE_ATT.Fixed:
                    //고정 데미지 일 시 감소 없음
                    break;
            }
            Debug.Log($"Atfer Calculator DamageAttribute = {amount}");
            return amount;
        }
        public void SetStat(StatsData _statData, float _currentHealth)
        {
            StatsData = _statData;
            CurrentHealth = _currentHealth;
            isSetup = true;
        }
        /// <summary>
        /// 버프 스탯 적용
        /// </summary>
        /// <param name="statsData"></param>
        public void AddStat(StatsData statsData)
        {
            m_statsData += statsData;
            OnChangeHealth?.Invoke();
            OnChangeStats?.Invoke();
        }

        /// <summary>
        /// 버프 스탯 제거
        /// </summary>
        /// <param name="statsData"></param>
        public void RemoveStat(StatsData statsData)
        {
            m_statsData += statsData * -1f;
            OnChangeHealth?.Invoke();
            OnChangeStats?.Invoke();
        }
    }
}