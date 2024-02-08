using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class WeaponProjectile : WeaponComponent<WeaponProjectileData, WeaponProjectileActionData>
    {
        public int currentProjectileIndex = 0;
        protected override void HandleEnter()
        {
            base.HandleEnter();
            currentProjectileIndex = 0;
        }

        private void HandleShootProjectile()
        {
            Debug.LogWarning($"{core.Unit.name}");

            if (currentActionData != null)
            {
                CheckAttackAction(currentActionData);
            }
            currentProjectileIndex++;
        }
        private void CheckAttackAction(WeaponProjectileActionData actionData)
        {
            if (actionData.ProjectileActionData.Length < currentProjectileIndex + 1)
                return;
            if (actionData.ProjectileActionData[currentProjectileIndex].ProjectileData.homingType == HomingType.isToTarget_Direct)
            {
                CoreMovement.FlipToTarget();
            }
            CoreEffectManager.StartProjectileCheck(core.Unit, actionData.ProjectileActionData[currentProjectileIndex]);
        }

        protected override void Start()
        {
            base.Start();            
            eventHandler.OnShootProjectile -= HandleShootProjectile;
            eventHandler.OnShootProjectile += HandleShootProjectile;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            eventHandler.OnShootProjectile -= HandleShootProjectile;
        }
    }
}
