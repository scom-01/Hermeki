using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCOM.CoreSystem;

namespace SCOM.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon Weapon
        {
            get
            {
                if (weapon == null)
                    weapon = this.GetComponent<Weapon>();
                return weapon;
            }
            set
            {
                weapon = value;
            }
        }
        private Weapon weapon;
        protected AnimationEventHandler eventHandler;
        protected Unit unit => Weapon?.GetComponentInParent<Unit>();
        protected Core core => Weapon.WeaponCore ?? Weapon.GetComponentInParent<Unit>().Core;
        protected bool isAttackActive;

        private CoreSystem.Movement coreMovement;
        protected CoreSystem.Movement CoreMovement
        {
            get => coreMovement ? coreMovement : core.GetCoreComponent(ref coreMovement);
        }
        private CoreSystem.CollisionSenses coreCollisionSenses;
        protected CoreSystem.CollisionSenses CoreCollisionSenses
        {
            get => coreCollisionSenses ? coreCollisionSenses : core.GetCoreComponent(ref coreCollisionSenses);
        }
        private CoreSystem.EffectManager coreEffectManager;
        protected CoreSystem.EffectManager CoreEffectManager
        {
            get => coreEffectManager ? coreEffectManager : core.GetCoreComponent(ref coreEffectManager);
        }

        private CoreSystem.UnitStats coreUnitStats;
        protected CoreSystem.UnitStats CoreUnitStats
        {
            get => coreUnitStats ? coreUnitStats : core.GetCoreComponent(ref coreUnitStats);
        }
        private CoreSystem.SoundEffect coreSoundEffect;
        protected CoreSystem.SoundEffect CoreSoundEffect
        {
            get => coreSoundEffect ? coreSoundEffect : core.GetCoreComponent(ref coreSoundEffect);
        }
        private CoreSystem.DamageReceiver coreDamageReceiver;
        protected CoreSystem.DamageReceiver CoreDamageReceiver
        {
            get => coreDamageReceiver ? coreDamageReceiver : core.GetCoreComponent(ref coreDamageReceiver);
        }
        private CoreSystem.DamageTransmitter coreDamageTransmitter;
        protected CoreSystem.DamageTransmitter CoreDamageTransmitter
        {
            get => coreDamageTransmitter ? coreDamageTransmitter : core.GetCoreComponent(ref coreDamageTransmitter);
        }
        private CoreSystem.KnockBackReceiver coreKnockBackReceiver;
        protected CoreSystem.KnockBackReceiver CoreKnockBackReceiver
        {
            get => coreKnockBackReceiver ? coreKnockBackReceiver : core.GetCoreComponent(ref coreKnockBackReceiver);
        }

        public virtual void Init()
        {

        }
        protected virtual void Awake()
        {
            Weapon.OnEnter += HandleEnter;
            Weapon.OnExit += HandleExit;
            eventHandler = GetComponentInChildren<AnimationEventHandler>();
        }

        protected virtual void Start()
        {
        }

        protected virtual void HandleEnter()
        {
            isAttackActive = true;
        }

        protected virtual void HandleExit()
        {
            isAttackActive = false;
        }

        protected virtual void OnDestroy()
        {
            Weapon.OnEnter -= HandleEnter;
            Weapon.OnExit -= HandleExit;
        }
    }
    public abstract class WeaponComponent<T1, T2> : WeaponComponent where T1 : ComponentData<T2> where T2 : ActionData
    {
        protected T1 data;
        protected T2 currentActionData;
        //protected T2 currentAirActionData;

        protected override void HandleEnter()
        {
            base.HandleEnter();
            if (data == null)
                return;

            if(data.ActionData.Length != 0)
            {
                currentActionData = data.ActionData[0] ?? null;
            }
            else
            {
                currentActionData = null;
            }
            //currentAirActionData = data.InAirActionData[weapon.CurrentActionCounter] ?? null;

        }

        public override void Init()
        {
            base.Init();

            data = Weapon.weaponData.weaponDataSO.GetData<T1>();
        }
    }
}