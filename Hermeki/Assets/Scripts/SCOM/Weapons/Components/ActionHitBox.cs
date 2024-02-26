using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class ActionHitBox : WeaponComponent<ActionHitBoxData, AttackActionHitBox>
    {
        //public event Action<Collider2D[]> OnDetectedCollider2D;

        private Vector2 offset;
        private Collider2D[] detected;
        private RaycastHit2D[] RayCastdetected;

        private bool isTriggerOn;
        private List<Collider2D> actionHitObjects = new List<Collider2D>();
        //private HitAction[] hitActions = null;

        private Vector2 PosOffset;

        public int currentHitBoxIndex = 0;
        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentHitBoxIndex = 0;
        }

        private void HandleAction()
        {
            if (currentActionData == null)
                return;

            //Action 시 효과
            core.Unit.Inventory.ItemActionExecute(core.Unit);
        }
        private void HandleAttackAction()
        {
            if (currentActionData != null)
            {
                CheckAttackAction(currentActionData);
            };
            currentHitBoxIndex++;
        }

        private void CheckAttackAction(AttackActionHitBox actionData)
        {
            if (actionData == null)
                return;

            var currHitBox = actionData.ActionHit;
            if (currHitBox.Length <= 0)
                return;

            offset.Set(
                    CoreCollisionSenses.GroundCenterPos.x + (currHitBox[currentHitBoxIndex].ActionRect.center.x * CoreMovement.FancingDirection),
                    CoreCollisionSenses.GroundCenterPos.y + (currHitBox[currentHitBoxIndex].ActionRect.center.y)
                    );

            detected = Physics2D.OverlapBoxAll(offset, currHitBox[currentHitBoxIndex].ActionRect.size, 0f, data.DetectableLayers);

            if (detected.Length == 0)
            {
                Debug.Log("detected.Length == 0");
                return;
            }

            #region HitAction Effect Spawn
            foreach (Collider2D coll in detected)
            {
                if (coll.gameObject.CompareTag(this.gameObject.tag))
                    continue;

                if (coll.gameObject.CompareTag("Trap"))
                    continue;

                //객체 사망 시 무시
                if (coll.gameObject.GetComponentInParent<Unit>().Core.CoreDeath.isDead)
                {
                    continue;
                }

                if (coll.gameObject.GetComponentInParent<Enemy>() != null)
                {
                    coll.gameObject.GetComponentInParent<Enemy>().SetTarget(core.Unit);
                }


                //Hit시 효과
                if (coll.TryGetComponent(out IDamageable victim))
                {
                    for (int j = 0; j < currHitBox[currentHitBoxIndex].RepeatAction; j++)
                    {
                        if (currHitBox[currentHitBoxIndex].isOnHit)
                        {
                            core.Unit.Inventory.ItemOnHitExecute(core.Unit, coll.GetComponentInParent<Unit>());
                        }

                        //EffectPrefab
                        #region EffectPrefab
                        if (currHitBox[currentHitBoxIndex].EffectPrefab != null)
                        {
                            for (int i = 0; i < currHitBox[currentHitBoxIndex].EffectPrefab.Length; i++)
                            {
                                if (currHitBox[currentHitBoxIndex].EffectPrefab[i].isRandomPosRot)
                                    victim.HitEffectRandRot(currHitBox[currentHitBoxIndex].EffectPrefab[i].Object, currHitBox[currentHitBoxIndex].EffectPrefab[i].isRandomRange, currHitBox[currentHitBoxIndex].EffectPrefab[i].EffectScale, currHitBox[currentHitBoxIndex].EffectPrefab[i].isFollowing);
                                else
                                    victim.HitEffect(currHitBox[currentHitBoxIndex].EffectPrefab[i].Object, currHitBox[currentHitBoxIndex].EffectPrefab[i].isRandomRange, CoreMovement.FancingDirection, currHitBox[currentHitBoxIndex].EffectPrefab[i].EffectScale);
                            }
                        }
                        #endregion

                        //AudioClip
                        #region AudioClip
                        if (currHitBox[currentHitBoxIndex].audioClips != null)
                        {
                            for (int i = 0; i < currHitBox[currentHitBoxIndex].audioClips.Length; i++)
                            {
                                CoreSoundEffect.AudioSpawn(currHitBox[currentHitBoxIndex].audioClips[i]);
                            }
                        }
                        #endregion

                    }//ShakeCam
                    #region ShakeCam
                    if (currHitBox[currentHitBoxIndex].camDatas != null)
                    {
                        for (int i = 0; i < currHitBox[currentHitBoxIndex].camDatas.Length; i++)
                        {
                            Camera.main.GetComponent<CameraShake>().Shake(
                                currHitBox[currentHitBoxIndex].camDatas[i].RepeatRate,
                                currHitBox[currentHitBoxIndex].camDatas[i].Range,
                                currHitBox[currentHitBoxIndex].camDatas[i].Duration
                                );
                        }
                    }
                    #endregion
                }

                //Damage
                if (coll.TryGetComponent(out IDamageable _victim))
                {
                    if (currHitBox[currentHitBoxIndex].isFixed)
                    {
                        _victim.FixedDamage(core.Unit, (int)currHitBox[currentHitBoxIndex].AdditionalDamage, true, currHitBox[currentHitBoxIndex].RepeatAction);
                    }
                    else
                    {
                        _victim.TypeCalDamage(core.Unit, CoreUnitStats.CalculStatsData.DefaultPower + currHitBox[currentHitBoxIndex].AdditionalDamage, currHitBox[currentHitBoxIndex].RepeatAction);
                    }
                }
                //KnockBack
                #region KnockBack
                if (coll.TryGetComponent(out IKnockBackable knockbackables))
                {
                    knockbackables.KnockBack(currHitBox[currentHitBoxIndex].KnockbackAngle, currHitBox[currentHitBoxIndex].KnockbackAngle.magnitude, CoreMovement.FancingDirection);
                }
                #endregion
            }
            #endregion
        }

        private void HandleActionRectOn()
        {
            PosOffset = core.Unit.transform.position;
            Debug.Log($"PosOffset = {PosOffset}");
            if (currentActionData != null)
            {
                CheckActionRect(currentActionData);
            }
        }

        private void HandleActionRectOff()
        {
            if (core.CoreDamageTransmitter != null)
                core.CoreDamageTransmitter.isTransmitter = false;
           
            currentHitBoxIndex++;
        }

        private void HandleMultipleActionRectOn()
        {
            PosOffset = core.Unit.transform.position;
            Debug.Log($"PosOffset = {PosOffset}");
            if (currentActionData != null)
            {
                CheckActionRect(currentActionData, false);
            }
        }

        private void HandleMultipleActionRectOff()
        {
            if (core.CoreDamageTransmitter != null)
                core.CoreDamageTransmitter.isTransmitter = false;
           
            currentHitBoxIndex++;
        }

        private void HandleRushActionRectOn()
        {
            PosOffset = core.Unit.transform.position;
            Debug.Log($"PosOffset = {PosOffset}");
            if (currentActionData != null)
            {
                CheckRushActionRect(currentActionData);
            }
        }

        //HandleRushActionRectOff()전에 OnTeleportToTarget을 해야한다
        private void HandleRushActionRectOff()
        {
            if (core.CoreDamageTransmitter != null)
                core.CoreDamageTransmitter.isTransmitter = false;

            //Vector2 oldPos = core.Unit.transform.position;
            //Debug.Log($"CurrPos = {oldPos}");
            //var temp = (PosOffset - oldPos);
            //Debug.Log($"Temp = {temp}");
            //offset.Set(
            //        CoreCollisionSenses.GroundCenterPos.x + (hitActions[currentHitBoxIndex].ActionRect.center.x * CoreMovement.FancingDirection),
            //        CoreCollisionSenses.GroundCenterPos.y + (hitActions[currentHitBoxIndex].ActionRect.center.y)
            //        );

            //float angle = Quaternion.Angle(Quaternion.Euler(PosOffset), Quaternion.Euler(oldPos));
            //RayCastdetected = Physics2D.BoxCastAll(offset, hitActions[currentHitBoxIndex].ActionRect.size, angle, temp, temp.magnitude, data.DetectableLayers);

            //#region HitAction Effect Spawn
            //for (int k = 0; k < RayCastdetected.Length; k++)
            //{
            //    var coll = RayCastdetected[k].collider;

            //    if (coll.gameObject.tag == this.gameObject.tag)
            //        continue;

            //    if (coll.gameObject.tag == "Trap")
            //        continue;

            //    //객체 사망 시 무시
            //    if (coll.gameObject.GetComponentInParent<Unit>().Core.CoreDeath.isDead)
            //    {
            //        continue;
            //    }

            //    if (coll.gameObject.GetComponentInParent<Enemy>() != null)
            //    {
            //        coll.gameObject.GetComponentInParent<Enemy>().SetTarget(core.Unit);
            //    }

            //    //Hit시 효과
            //    if (coll.TryGetComponent(out IDamageable victim))
            //    {
            //        for (int j = 0; j < hitActions[currentHitBoxIndex].RepeatAction; j++)
            //        {
            //            core.Unit.Inventory.ItemOnHitExecute(core.Unit, coll.GetComponentInParent<Unit>());

            //            //EffectPrefab
            //            #region EffectPrefab
            //            if (hitActions[currentHitBoxIndex].EffectPrefab != null)
            //            {
            //                for (int i = 0; i < hitActions[currentHitBoxIndex].EffectPrefab.Length; i++)
            //                {
            //                    if (hitActions[currentHitBoxIndex].EffectPrefab[i].isRandomPosRot)
            //                        victim.HitEffectRandRot(hitActions[currentHitBoxIndex].EffectPrefab[i].Object, hitActions[currentHitBoxIndex].EffectPrefab[i].isRandomRange, hitActions[currentHitBoxIndex].EffectPrefab[i].EffectScale, hitActions[currentHitBoxIndex].EffectPrefab[i].isFollowing);
            //                    else
            //                        victim.HitEffect(hitActions[currentHitBoxIndex].EffectPrefab[i].Object, hitActions[currentHitBoxIndex].EffectPrefab[i].isRandomRange, CoreMovement.FancingDirection, hitActions[currentHitBoxIndex].EffectPrefab[i].EffectScale);
            //                }
            //            }
            //            #endregion

            //            //AudioClip
            //            #region AudioClip
            //            if (hitActions[currentHitBoxIndex].audioClips != null)
            //            {
            //                for (int i = 0; i < hitActions[currentHitBoxIndex].audioClips.Length; i++)
            //                {
            //                    CoreSoundEffect.AudioSpawn(hitActions[currentHitBoxIndex].audioClips[i]);
            //                }
            //            }
            //            #endregion

            //        }//ShakeCam
            //        #region ShakeCam
            //        if (hitActions[currentHitBoxIndex].camDatas != null)
            //        {
            //            for (int i = 0; i < hitActions[currentHitBoxIndex].camDatas.Length; i++)
            //            {
            //                Camera.main.GetComponent<CameraShake>().Shake(
            //                    hitActions[currentHitBoxIndex].camDatas[i].RepeatRate,
            //                    hitActions[currentHitBoxIndex].camDatas[i].Range,
            //                    hitActions[currentHitBoxIndex].camDatas[i].Duration
            //                    );
            //            }
            //        }
            //        #endregion
            //    }

            //    //Damage
            //    if (coll.TryGetComponent(out IDamageable _victim))
            //    {
            //        if (hitActions[currentHitBoxIndex].isFixed)
            //        {
            //            _victim.FixedDamage(core.Unit, (int)hitActions[currentHitBoxIndex].AdditionalDamage, true, hitActions[currentHitBoxIndex].RepeatAction);
            //        }
            //        else
            //        {
            //            _victim.TypeCalDamage(core.Unit, CoreUnitStats.CalculStatsData.DefaultPower + hitActions[currentHitBoxIndex].AdditionalDamage, hitActions[currentHitBoxIndex].RepeatAction);
            //        }
            //    }

            //    //KnockBack
            //    #region KnockBack
            //    if (coll.TryGetComponent(out IKnockBackable knockbackables) && hitActions[currentHitBoxIndex].KnockbackAngle.magnitude > 0)
            //    {
            //        knockbackables.KnockBack(hitActions[currentHitBoxIndex].KnockbackAngle, hitActions[currentHitBoxIndex].KnockbackAngle.magnitude, CoreMovement.FancingDirection);
            //    }
            //    #endregion

            //}
            //#endregion

            //foreach (var obj in actionHitObjects)
            //{
            //    Debug.Log($"공격 받았던 오브젝트 {obj.name}");
            //}
            //actionHitObjects.Clear();
            //hitActions = null;
            //isTriggerOn = false;

            currentHitBoxIndex++;
        }

        private void CheckActionRect(AttackActionHitBox actionData, bool isSingle = true)
        {
            if (actionData == null)
                return;

            var currHitBox = actionData.ActionHit;
            if (currHitBox.Length <= 0)
                return;
            if (core.CoreDamageTransmitter != null)
            {
                core.CoreDamageTransmitter.SetCollider2D(currHitBox[currentHitBoxIndex], true, isSingle);
                core.CoreDamageTransmitter.isTransmitter = true;
            }
        }
        private void CheckRushActionRect(AttackActionHitBox actionData, bool isSingle = true)
        {
            if (actionData == null)
                return;

            var currHitBox = actionData.ActionHit;
            if (currHitBox.Length <= 0)
                return;

            if (core.CoreDamageTransmitter != null)
            {
                core.CoreDamageTransmitter.SetCollider2D(currHitBox[currentHitBoxIndex], true, isSingle);
                core.CoreDamageTransmitter.isTransmitter = true;
            }
        }

        private void AttMessageBox()
        {
            if (currentActionData == null)
                return;
            #region SpawnAttackMessageBox

            (GameManager.Inst.StageManager.EffectContainer?.CheckObject(ObjectPooling_TYPE.Effect, GlobalValue.Base_AttackMessageBox, currentActionData.ActionHit[currentHitBoxIndex].ActionRect.size, GameManager.Inst.StageManager.EffectContainer.transform) as EffectPooling).
                GetObejct(
                new Vector2(
                    transform.position.x + (currentActionData.ActionHit[currentHitBoxIndex].ActionRect.center.x * CoreMovement.FancingDirection),
                    transform.position.y + (currentActionData.ActionHit[currentHitBoxIndex].ActionRect.center.y)),
                Quaternion.identity, currentActionData.ActionHit[currentHitBoxIndex].ActionRect.size);
            #endregion
        }
        protected override void Start()
        {
            base.Start();
            eventHandler.OnAttMessageBox -= AttMessageBox;
            eventHandler.OnAttMessageBox += AttMessageBox;

            eventHandler.OnAttackAction -= HandleAttackAction;
            eventHandler.OnAttackAction += HandleAttackAction;
            eventHandler.OnAttackAction -= HandleAction;
            eventHandler.OnAttackAction += HandleAction;

            eventHandler.OnActionRectOn -= HandleActionRectOn;
            eventHandler.OnActionRectOn += HandleActionRectOn;
            eventHandler.OnActionRectOn -= HandleAction;
            eventHandler.OnActionRectOn += HandleAction;

            eventHandler.OnMultipleActionRectOn -= HandleMultipleActionRectOn;
            eventHandler.OnMultipleActionRectOff += HandleMultipleActionRectOff;

            eventHandler.OnActionRectOff -= HandleActionRectOff;
            eventHandler.OnActionRectOff += HandleActionRectOff;

            eventHandler.OnRushActionRectOn -= HandleRushActionRectOn;
            eventHandler.OnRushActionRectOn += HandleRushActionRectOn;
            eventHandler.OnRushActionRectOn -= HandleAction;
            eventHandler.OnRushActionRectOn += HandleAction;

            eventHandler.OnRushActionRectOff -= HandleRushActionRectOff;
            eventHandler.OnRushActionRectOff += HandleRushActionRectOff;

            //만약 DamageTransmitter의 Collider2D가 enable = true인 상태로 종료되는 오류 방지
            eventHandler.OnFinish += HandleActionRectOff;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            eventHandler.OnAttMessageBox -= AttMessageBox;

            eventHandler.OnAttackAction -= HandleAttackAction;
            eventHandler.OnAttackAction -= HandleAction;

            eventHandler.OnActionRectOn -= HandleActionRectOn;
            eventHandler.OnActionRectOn -= HandleAction;

            eventHandler.OnMultipleActionRectOn -= HandleMultipleActionRectOn;
            eventHandler.OnMultipleActionRectOn -= HandleAction;

            eventHandler.OnActionRectOff -= HandleActionRectOff;
            
            eventHandler.OnMultipleActionRectOff -= HandleMultipleActionRectOff;

            eventHandler.OnRushActionRectOn -= HandleRushActionRectOn;

            eventHandler.OnRushActionRectOff -= HandleRushActionRectOff;

            eventHandler.OnFinish -= HandleActionRectOff;
        }

        private void OnDrawGizmos()
        {
            if (data == null)
                return;

            foreach (var item in data.ActionData)
            {
                if (item.ActionHit == null)
                    continue;

                foreach (var action in item.ActionHit)
                {
                    if (!action.Debug)
                        continue;
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(CoreCollisionSenses.GroundCenterPos + new Vector3(action.ActionRect.center.x * CoreMovement.FancingDirection, action.ActionRect.center.y, 0), action.ActionRect.size);
                }
            }
        }
    }
}