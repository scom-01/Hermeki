using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SCOM.CoreSystem
{
    public class DamageTransmitter : CoreComponent
    {
        private CoreComp<UnitStats> stats;
        private CoreComp<EffectManager> effectManager;
        private CoreComp<Death> death;
        private BoxCollider2D BC2D
        {
            get
            {
                if (bc2d == null)
                {
                    bc2d = this.GetComponent<BoxCollider2D>();
                    if (bc2d == null)
                    {
                        bc2d = this.AddComponent<BoxCollider2D>();
                    }
                    bc2d.enabled = false;
                }
                return bc2d;
            }
            set => bc2d = value;
        }
        private BoxCollider2D bc2d;

        private CircleCollider2D CC2D
        {
            get
            {
                if (cc2d == null)
                {
                    cc2d = this.GetComponent<CircleCollider2D>();
                    if (cc2d == null)
                    {
                        cc2d = this.AddComponent<CircleCollider2D>();
                    }
                    cc2d.enabled = false;
                }
                return cc2d;
            }
            set => cc2d = value;
        }
        private CircleCollider2D cc2d;

        /// <summary>
        /// 현재 사용중인 Collider2D
        /// </summary>
        private Collider2D CurrentCD;
        private HitAction HitAction;

        private List<Unit> HitUnits = new List<Unit>();

        /// <summary>
        /// OnTriggerOn;
        /// </summary>
        public bool isTransmitter
        {
            get => _isTransmitter;
            set
            {
                _isTransmitter = value;
                if (CurrentCD != null)
                    CurrentCD.enabled = _isTransmitter;
            }
        }

        private bool _isTransmitter;

        /// <summary>
        /// 단일공격 여부
        /// </summary>
        public bool isSingle;
        public bool isBox
        {
            get => _isBox;
            set
            {
                _isBox = value;

                if (CurrentCD != null)
                    CurrentCD.enabled = false;

                if (_isBox)
                {
                    CurrentCD = BC2D;
                }
                else
                {
                    CurrentCD = CC2D;
                }

            }
        }
        private bool _isBox;
        protected override void Awake()
        {
            base.Awake();
            BC2D.offset = core.Unit.CC2D.offset;
            BC2D.size = core.Unit.CC2D.size;
            BC2D.isTrigger = true;
            BC2D.enabled = false;
            CC2D.offset = core.Unit.CC2D.offset;
            CC2D.isTrigger = true;
            CC2D.enabled = false;
            stats = new CoreComp<UnitStats>(core);
            effectManager = new CoreComp<EffectManager>(core);
            death = new CoreComp<Death>(core);
            this.tag = core.Unit.gameObject.tag;
        }

        public void SetCollider2D(HitAction hitAction, bool _isBox = true, bool _isSingle = true)
        {
            HitUnits.Clear();
            HitAction = hitAction;
            isSingle = _isSingle;
            SetC2D(HitAction.ActionRect.center, HitAction.ActionRect.size, _isBox);
        }

        private void SetC2D(Vector2 _offset, Vector2 _size, bool _isbox)
        {
            isBox = _isbox;
            var temp = core.CoreCollisionSenses.GroundCenterPos - this.transform.position;
            if (isBox)
            {
                BC2D.size = _size;
                //HitAction.Rect는 설정이 pivot (0, 0)을 기준으로 세팅되어 있기에 (0.5, 0.5)로 맞춰주기 위함                
                BC2D.offset = new Vector2(temp.x * core.CoreMovement.FancingDirection, temp.y) + _offset;
            }
            else
            {
                CC2D.radius = _size.x;
                CC2D.offset = new Vector2(temp.x * core.CoreMovement.FancingDirection, temp.y) + _offset;
            }
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!isTransmitter)
                return;

            //단일공격이 아닐 때는 return
            if (!isSingle)
                return;

            if (coll.CompareTag(this.tag))
                return;

            var DmgReceiver = coll.GetComponent<DamageReceiver>();
            if (DmgReceiver == null)
                return;


            //객체 사망 시 무시
            if (coll.gameObject.GetComponentInParent<Unit>().Core.CoreDeath.isDead)
            {
                return;
            }

            if (HitUnits.Contains(coll.gameObject.GetComponentInParent<Unit>()))
            {
                return;
            }
            HitUnits.Add(coll.gameObject.GetComponentInParent<Unit>());

            if (coll.gameObject.GetComponentInParent<Enemy>() != null)
            {
                coll.gameObject.GetComponentInParent<Enemy>().SetTarget(core.Unit);
            }

            var temp = 0f;
            if (coll.TryGetComponent(out IDamageable _victim))
            {
                if (HitAction.isFixed)
                {
                    temp = _victim.FixedDamage(core.Unit, (int)HitAction.AdditionalDamage, true, HitAction.RepeatAction);
                }
                else
                {
                    temp = _victim.TypeCalDamage(core.Unit, core.CoreUnitStats.CalculStatsData.DefaultPower + HitAction.AdditionalDamage, HitAction.RepeatAction);
                }

                if (temp > 0f)
                {
                    //Hit시 효과
                    if (coll.TryGetComponent(out IDamageable victim))
                    {
                        for (int j = 0; j < HitAction.RepeatAction; j++)
                        {
                            if(HitAction.isOnHit)
                            {
                                core.Unit.Inventory.ItemOnHitExecute(core.Unit, coll.GetComponentInParent<Unit>());
                            }

                            //EffectPrefab
                            #region EffectPrefab
                            if (HitAction.EffectPrefab != null)
                            {
                                for (int i = 0; i < HitAction.EffectPrefab.Length; i++)
                                {
                                    if (HitAction.EffectPrefab[i].isRandomPosRot)
                                        victim.HitEffectRandRot(HitAction.EffectPrefab[i].Object, HitAction.EffectPrefab[i].isRandomRange, HitAction.EffectPrefab[i].EffectScale, HitAction.EffectPrefab[i].isFollowing);
                                    else
                                        victim.HitEffect(HitAction.EffectPrefab[i].Object, HitAction.EffectPrefab[i].isRandomRange, core.CoreMovement.FancingDirection, HitAction.EffectPrefab[i].EffectScale);
                                }
                            }
                            #endregion

                            //AudioClip
                            #region AudioClip
                            if (HitAction.audioClips != null)
                            {
                                for (int i = 0; i < HitAction.audioClips.Length; i++)
                                {
                                    core.CoreSoundEffect.AudioSpawn(HitAction.audioClips[i]);
                                }
                            }
                            #endregion

                        }//ShakeCam
                        #region ShakeCam
                        if (HitAction.camDatas != null)
                        {
                            for (int i = 0; i < HitAction.camDatas.Length; i++)
                            {
                                Camera.main.GetComponent<CameraShake>().Shake(
                                    HitAction.camDatas[i].RepeatRate,
                                    HitAction.camDatas[i].Range,
                                    HitAction.camDatas[i].Duration
                                    );
                            }
                        }
                        #endregion
                    }
                }
            }

            //KnockBack
            #region KnockBack
            if (coll.TryGetComponent(out IKnockBackable knockbackables))
            {
                knockbackables.KnockBack(HitAction.KnockbackAngle, HitAction.KnockbackAngle.magnitude, core.CoreMovement.FancingDirection);
            }
            #endregion
        }

        private void OnTriggerStay2D(Collider2D coll)
        {
            if (!isTransmitter)
                return;

            //단일공격이 일 때는 return
            if (isSingle)
                return;

            if (coll.CompareTag(this.tag))
                return;


            var DmgReceiver = coll.GetComponent<DamageReceiver>();
            if (DmgReceiver == null)
                return;

            //객체 사망 시 무시
            if (coll.gameObject.GetComponentInParent<Unit>().Core.CoreDeath.isDead)
            {
                return;
            }

            if (coll.gameObject.GetComponentInParent<Enemy>() != null)
            {
                coll.gameObject.GetComponentInParent<Enemy>().SetTarget(core.Unit);
            }

            var temp = 0f;
            if (coll.TryGetComponent(out IDamageable _victim))
            {
                if (HitAction.isFixed)
                {
                    temp = _victim.FixedDamage(core.Unit, (int)HitAction.AdditionalDamage, true, HitAction.RepeatAction);
                }
                else
                {
                    temp = _victim.TypeCalDamage(core.Unit, core.CoreUnitStats.CalculStatsData.DefaultPower + HitAction.AdditionalDamage, HitAction.RepeatAction);
                }

                if (temp > 0f)
                {
                    //Hit시 효과
                    if (coll.TryGetComponent(out IDamageable victim))
                    {
                        for (int j = 0; j < HitAction.RepeatAction; j++)
                        {
                            if(HitAction.isOnHit)
                            {
                                core.Unit.Inventory.ItemOnHitExecute(core.Unit, coll.GetComponentInParent<Unit>());
                            }

                            //EffectPrefab
                            #region EffectPrefab
                            if (HitAction.EffectPrefab != null)
                            {
                                for (int i = 0; i < HitAction.EffectPrefab.Length; i++)
                                {
                                    if (HitAction.EffectPrefab[i].isRandomPosRot)
                                        victim.HitEffectRandRot(HitAction.EffectPrefab[i].Object, HitAction.EffectPrefab[i].isRandomRange, HitAction.EffectPrefab[i].EffectScale, HitAction.EffectPrefab[i].isFollowing);
                                    else
                                        victim.HitEffect(HitAction.EffectPrefab[i].Object, HitAction.EffectPrefab[i].isRandomRange, core.CoreMovement.FancingDirection, HitAction.EffectPrefab[i].EffectScale);
                                }
                            }
                            #endregion

                            //AudioClip
                            #region AudioClip
                            if (HitAction.audioClips != null)
                            {
                                for (int i = 0; i < HitAction.audioClips.Length; i++)
                                {
                                    core.CoreSoundEffect.AudioSpawn(HitAction.audioClips[i]);
                                }
                            }
                            #endregion

                        }//ShakeCam
                        #region ShakeCam
                        if (HitAction.camDatas != null)
                        {
                            for (int i = 0; i < HitAction.camDatas.Length; i++)
                            {
                                Camera.main.GetComponent<CameraShake>().Shake(
                                    HitAction.camDatas[i].RepeatRate,
                                    HitAction.camDatas[i].Range,
                                    HitAction.camDatas[i].Duration
                                    );
                            }
                        }
                        #endregion
                    }
                }
            }

            //KnockBack
            #region KnockBack
            if (coll.TryGetComponent(out IKnockBackable knockbackables))
            {
                knockbackables.KnockBack(HitAction.KnockbackAngle, HitAction.KnockbackAngle.magnitude, core.CoreMovement.FancingDirection);
            }
            #endregion
        }
    }
}