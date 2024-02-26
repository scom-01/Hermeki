using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons.Components
{
    public class Knockback : WeaponComponent<KnockbackData, ActionKnockback>
    {
        private ActionHitBox hitBox;
        private void HandleDetectedCollider2D(Collider2D[] coll)
        {
            if (currentActionData != null)
            {
                CheckKnockbackAction(currentActionData, coll);
            }
            //if (currentActionData != null && currentAirActionData != null)
            //{
            //    if (weapon.InAir)
            //    {
            //        CheckKnockbackAction(currentAirActionData, coll);
            //    }
            //    else
            //    {
            //        CheckKnockbackAction(currentActionData, coll);
            //    }
            //}
            //else if (currentActionData == null)
            //{
            //    CheckKnockbackAction(currentAirActionData, coll);
            //}
            //else if (currentAirActionData == null)
            //{
            //    CheckKnockbackAction(currentActionData, coll);
            //}
        }
        private void CheckKnockbackAction(ActionKnockback actionKnockback, Collider2D[] coll)
        {
            if (actionKnockback == null)
                return;

            var currKnockback = actionKnockback.KnockbackAngle;
            if (currKnockback.Length <= 0)
                return;

            if (currKnockback.Length <= hitBox.currentHitBoxIndex)
                return;

            foreach (var detecte in coll)
            {
                if (detecte.gameObject.CompareTag(this.gameObject.tag))
                    continue;

                if (detecte.TryGetComponent(out IKnockBackable knockbackables))
                {
                    knockbackables.KnockBack(currKnockback[hitBox.currentHitBoxIndex], currKnockback[hitBox.currentHitBoxIndex].magnitude, CoreMovement.FancingDirection);
                }
            }
        }
        protected override void Awake()
        {
            base.Awake();

            hitBox = GetComponent<ActionHitBox>();
            //hitBox.OnDetectedCollider2D -= HandleDetectedCollider2D;
            //hitBox.OnDetectedCollider2D += HandleDetectedCollider2D;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            //hitBox.OnDetectedCollider2D -= HandleDetectedCollider2D;
        }
    }
}
