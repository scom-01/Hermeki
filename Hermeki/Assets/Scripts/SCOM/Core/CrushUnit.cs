using SCOM.CoreSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrushUnit : CoreComponent
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (core?.Unit == null)
            return;

        //사망 시
        if (!core.Unit.IsAlive)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable") && collision.tag != this.tag) 
        {
            if (collision.TryGetComponent(out IDamageable victim))
            {
                victim.Damage(core.Unit, 1, 1);
            }
        }
    }
}
