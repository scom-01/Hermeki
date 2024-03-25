using UnityEngine;
using UnityEngine.U2D.IK;

namespace SCOM.CoreSystem
{
    public class DamageReceiver : CoreComponent, IDamageable
    {
        public GameObject DefaultEffectPrefab;

        private CoreComp<UnitStats> stats;
        private CoreComp<EffectManager> effectManager;
        private CoreComp<Death> death;
        private BoxCollider2D BC2D;
        private CapsuleCollider2D CC2D;
        //객체가 이미 Hit를 했는지 판별(ex. true면 이미 피격당했다고 생각함)
        public bool isHit
        {
            get
            {
                if (core.Unit.Get_Fixed_Hit_Immunity)
                {
                    return true;
                }

                return ishit;
            }
            set
            {
                if (core.Unit.Get_Fixed_Hit_Immunity)
                {
                    ishit = true;
                }
                else
                {
                    ishit = value;
                }
            }
        }
        private bool ishit = false;
        public bool isTouch
        {
            get => istouch;
            set
            {
                istouch = value;
            }
        }
        private bool istouch = false;

        public float Damage(Unit attacker, float amount, int repeat = 1)
        {
            if (death.Comp.isDead)
            {
                Debug.Log(core.Unit.name + "is Dead");
                return 0f;
            }

            if (CheckHitImmunity(attacker))
            {
                return 0f;
            }

            if (CheckHit(attacker))
            {
                return 0f;
            }

            //for (int i = 0; i < repeat; i++)
            //{
            //    temp += TrueDamage(attacker, amount);
            //}
            float temp = stats.Comp.DecreaseHealth(attacker, amount);
            stats.Comp.invincibleTime = core.Unit.UnitData.invincibleTime;
            isHit = true;
            return temp;
        }
        /// <summary>
        /// 히트 무적 무시
        /// </summary>
        /// <param name="AttackterCommonData"></param>
        /// <param name="VictimCommonData"></param>
        /// <param name="amount"></param>
        public float TrueDamage(Unit attacker, float amount)
        {
            return TrueDamage(attacker, attacker.Core.CoreUnitStats.CalculStatsData.Elemental, attacker.Core.CoreUnitStats.CalculStatsData.DamageAttiribute, amount);
        }
        public float TrueDamage(Unit attacker, E_Power _elemental, DAMAGE_ATT attribute, float amount, bool CanCritical = true)
        {
            if (death.Comp.isDead)
            {
                Debug.Log(core.Unit.name + "is Dead");
                return 0f;
            }

            if (CheckHitImmunity(attacker))
            {
                return 0f;
            }

            bool isCritical = false;

            if (CanCritical)
            {
                //크리티컬 계산
                if (CheckCritical(attacker))
                {
                    isCritical = true;
                    amount *= 1f + (attacker.Core.CoreUnitStats.CalculStatsData.CriticalDmgPer / 100.0f);
                }
            }

            Debug.Log(core.transform.parent.name + " " + amount + " Damaged!");
            var damage = stats.Comp.DecreaseHealth(attacker, _elemental, attribute, amount);
            stats.Comp.invincibleTime = core.Unit.UnitData.invincibleTime;
            //HUD_DmgTxt(1.0f, damage, 50, attacker.Core.CoreUnitStats.CalculStatsData.DamageAttiribute, isCritical);
            return damage;
        }

        public float TypeCalDamage(Unit attacker, float attackerDmg, int RepeatAmount = 1)
        {
            float temp = attackerDmg;
            if ((int)attacker.UnitData.unit_size > (int)core.Unit.UnitData.unit_size)
            {
                temp *= (1.0f + GlobalValue.Enemy_Size_WeakPer);
                Debug.Log($"Attacker Unit Size Bigger than {core.Unit.name}, Calculate Befor Dmg = {attackerDmg}, after Dmg = {temp}");
            }
            else if ((int)attacker.UnitData.unit_size == (int)core.Unit.UnitData.unit_size)
            {
                Debug.Log($"Attacker Unit Size equal to {core.Unit.name}");
            }
            else if ((int)attacker.UnitData.unit_size < (int)core.Unit.UnitData.unit_size)
            {
                temp *= (1.0f - GlobalValue.Enemy_Size_WeakPer);
                Debug.Log($"Attacker Unit Size smaller than {core.Unit.name}, Calculate Befor Dmg = {attackerDmg}, after Dmg = {temp}");
            }

            return Damage(attacker, temp, RepeatAmount);
        }

        public float FixedDamage(Unit attacker, int amount, bool isTrueHit = false, int RepeatAmount = 1)
        {
            if (death.Comp.isDead)
            {
                Debug.Log(core.Unit.name + "is Dead");
                return 0f;
            }

            if (CheckHitImmunity(attacker))
            {
                return 0f;
            }

            float damage = 0;
            if (isTrueHit)
            {
                damage = stats.Comp.DecreaseHealth(attacker, DAMAGE_ATT.Fixed, amount);
                HUD_DmgTxt(1.0f, damage, 50, DAMAGE_ATT.Fixed, false);
                if (RepeatAmount > 1)
                {
                    var temp = RepeatAmount - 1;
                    return FixedDamage(attacker, amount, isTrueHit, temp) + damage;
                }
            }
            else
            {
                if (CheckHit(attacker))
                {
                    Debug.Log(core.Unit.name + " isHit = true");
                    return 0f;
                }
                damage = stats.Comp.DecreaseHealth(attacker, DAMAGE_ATT.Fixed, amount);
                HUD_DmgTxt(1.0f, damage, 50, DAMAGE_ATT.Fixed, false);
                if (damage > 0)
                {
                    isHit = true;
                }
            }
            if (damage > 0)
            {
                Debug.Log(core.transform.parent.name + " " + damage + " Damaged!");
                stats.Comp.invincibleTime = core.Unit.UnitData.invincibleTime;
            }
            return damage;
        }
        /// <summary>
        /// 고정 데미지
        /// </summary>
        /// <param name="amount">데미지 량</param>
        /// <param name="isTrueHit">피격 후 무적판정 무시공격(true)</param>
        /// <param name="RepeatAmount">반복 횟수(isTrueHit = false 때는 반영되지 않음)</param>
        public float FixedDamage(int amount, bool isTrueHit = false, int RepeatAmount = 1)
        {
            return FixedDamage(null, amount, isTrueHit, RepeatAmount);
        }
        public float TrapDamage(StatsData AttackterCommonData, float amount)
        {
            if (death.Comp.isDead)
            {
                Debug.Log(core.Unit.name + "is Dead");
                return 0f;
            }
            if (CheckHitImmunity(null))
            {
                return 0f;
            }
            if (isTouch)
            {
                return 0f;
            }

            Debug.Log(core.transform.parent.name + " " + amount + " Damaged!");
            isTouch = true;
            var damage = stats.Comp.DecreaseHealth(AttackterCommonData.Elemental, AttackterCommonData.DamageAttiribute, amount);
            if (damage > 0)
            {
                stats.Comp.TouchinvincibleTime = core.Unit.UnitData.touchDamageinvincibleTime;
            }
            HUD_DmgTxt(1.0f, damage, 50, AttackterCommonData.DamageAttiribute);
            return damage;
        }
        public void HitEffect(GameObject EffectPrefab, float Range, int FancingDirection, Vector3 size)
        {
            if (EffectPrefab == null)
            {
                if (DefaultEffectPrefab != null)
                {
                    effectManager.Comp.StartEffectsWithRandomPos(DefaultEffectPrefab, Range, FancingDirection, size);
                }
                return;
            }

            effectManager.Comp.StartEffectsWithRandomPos(EffectPrefab, Range, FancingDirection, size);
        }
        public void HitEffectRandRot(GameObject EffectPrefab, float Range, Vector3 size, bool isFollow = false)
        {
            if (EffectPrefab == null)
            {
                if (DefaultEffectPrefab != null)
                {
                    effectManager.Comp.StartEffectsWithRandomPosRot(DefaultEffectPrefab, Range, size, isFollow);
                }
                return;
            }

            effectManager.Comp.StartEffectsWithRandomPosRot(EffectPrefab, Range, size, isFollow);
        }

        #region DmgTxt
        /// <summary>
        /// Random위치에 파티클을 생성하고 UI상으로 같은 위치에 DamageText를 생성하는 로직
        /// </summary>
        /// <param name="effectPrefab">생성할 파티클</param>
        /// <param name="range">피격위치로부터 랜덤 위치값을 가져올 구 범위의 반지름</param>
        /// <param name="damage">피격 데미지</param>
        /// <param name="fontSize">DamageText 폰트 사이즈</param>
        /// <param name="damageAttiribute">Damage속성</param>
        /// <returns></returns>
        public GameObject HUD_DmgTxt(GameObject effectPrefab, float range, float damage, float fontSize, DAMAGE_ATT damageAttiribute, bool isCritical = false)
        {
            var effect = effectManager.Comp?.StartEffectsWithRandomPos(effectPrefab, range, Vector3.one, effectManager.Comp.transform.position);

            return HUD_DmgTxt(range, damage, fontSize, damageAttiribute, isCritical);
        }

        public GameObject HUD_DmgTxt(float damage, float fontSize, DAMAGE_ATT damageAttiribute, bool isCritical = false)
        {
            return HUD_DmgTxt(0, damage, fontSize, damageAttiribute, isCritical);
        }

        public GameObject HUD_DmgTxt(float range, float damage, float fontSize, DAMAGE_ATT damageAttiribute)
        {
            return HUD_DmgTxt(range, damage, fontSize, damageAttiribute, false);
        }
        public GameObject HUD_DmgTxt(float range, float damage, float fontSize, DAMAGE_ATT damageAttiribute, bool isCritical = false)
        {
            if (damage <= 0)
                return null;
            var randomPos = new Vector2(transform.position.x + Random.Range(-range, range), transform.position.y + Random.Range(-range, range));

            var pos = new Vector2((Camera.main.WorldToViewportPoint(randomPos).x * GameManager.Inst.DamageUI.GetComponent<RectTransform>().sizeDelta.x) - (GameManager.Inst.DamageUI.GetComponent<RectTransform>().sizeDelta.x * 0.5f),
                                    (Camera.main.WorldToViewportPoint(randomPos).y * GameManager.Inst.DamageUI.GetComponent<RectTransform>().sizeDelta.y) - (GameManager.Inst.DamageUI.GetComponent<RectTransform>().sizeDelta.y * 0.5f));

            GameObject damageText;

            if (isCritical)
            {
                damageText = (GameManager.Inst.StageManager.EffectContainer.CheckObject(ObjectPooling_TYPE.DmgText, GlobalValue.CriticalDamageTextPrefab, Vector3.one) as DmgTxtPooling).GetObejct(new Vector3(pos.x, pos.y), Quaternion.identity, damage, fontSize, damageAttiribute);
                if (GlobalValue.CriticalHit_SFX != null)
                    GameManager.Inst.StageManager?.player?.Core.CoreSoundEffect.AudioSpawn(GlobalValue.CriticalHit_SFX);
            }
            else
            {
                damageText = (GameManager.Inst.StageManager.EffectContainer.CheckObject(ObjectPooling_TYPE.DmgText, GlobalValue.DamageTextPrefab, Vector3.one) as DmgTxtPooling).GetObejct(new Vector3(pos.x, pos.y), Quaternion.identity, damage, fontSize, damageAttiribute);
            }
            return damageText;
        }
        #endregion

        #region Check Func

        private bool CheckHitImmunity(Unit attacker)
        {
            if (core.Unit.Get_Fixed_Hit_Immunity)
            {
                //core.Unit.Inventory?.ItemExeOnDodge(core.Unit, attacker);
                return true;
            }
            return false;
        }
        private bool CheckHit(Unit attacker)
        {
            if (isHit)
            {
                return true;
            }
            return false;
        }

        private bool CheckCritical(Unit attacker)
        {
            if (attacker.Core.CoreUnitStats.CalculStatsData.CriticalPer >= Random.Range(0, 100.0f))
            {
                //core.Unit.Inventory?.ItemExeOnCritical(core.Unit, attacker);
                return true;
            }
            return false;
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            CC2D = GetComponent<CapsuleCollider2D>();
            if (CC2D != null)
            {
                CC2D.isTrigger = true;
                CC2D.offset = core.Unit.CC2D.offset;
                CC2D.size = core.Unit.CC2D.size;
            }
            BC2D = GetComponent<BoxCollider2D>();
            if (BC2D != null)
            {
                BC2D.isTrigger = true;
                BC2D.offset = core.Unit.CC2D.offset;
                BC2D.size = core.Unit.CC2D.size;
            }
            stats = new CoreComp<UnitStats>(core);
            effectManager = new CoreComp<EffectManager>(core);
            death = new CoreComp<Death>(core);
            this.tag = core.Unit.gameObject.tag;
        }
    }
}