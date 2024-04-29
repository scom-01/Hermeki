using UnityEngine;


public class DeadArea : TouchObject
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        Unit unit = collision.gameObject.GetComponent<Unit>();
        unit.Core?.CoreMovement.SetVelocityZero();

        unit.Core.CoreDeath.Die();
        ////지정된 리스폰 위치로 이동
        //if (GameManager.Inst?.StageManager?.SpawnPoint != null)
        //    unit.gameObject.transform.position = unit.RespawnPoint.position;

        //var amount = unit.Core.CoreUnitStats.DecreaseHealth(E_Power.Normal, DAMAGE_ATT.Fixed, 10);
        //if (unit.Core.CoreDamageReceiver.DefaultEffectPrefab == null)
        //{
        //    unit.Core.CoreDamageReceiver.HUD_DmgTxt(0.5f, amount, 50, DAMAGE_ATT.Fixed);
        //}
        //else
        //{
        //    unit.Core.CoreDamageReceiver.HUD_DmgTxt(unit.Core.CoreDamageReceiver.DefaultEffectPrefab, 0.5f, amount, 50, DAMAGE_ATT.Fixed, false);
        //}
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {

    }
}
