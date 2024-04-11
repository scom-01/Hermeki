using System;
using UnityEngine;

public abstract class WeaponVariety
{
    public WeaponItem WeaponItem;
    public abstract void Action();
}
public class SwordVariety : WeaponVariety
{
    public override void Action()
    {
        throw new NotImplementedException();
    }
}
public class StaffVariety : WeaponVariety
{
    public override void Action()
    {
        WeaponItem.DecreaseDurability();
    }
}
public class SpearVariety : WeaponVariety
{
    public override void Action()
    {
        GameObject result = GameManager.Inst.LevelManager.CurrStage().SO_Controller.GetSpawnItem(WeaponItem.Data, WeaponItem.transform.position);
        result.tag = "Player";
        if (result.GetComponentInChildren<EquipObject>() != null)
        {
            result.transform.localScale = Vector3.one * 1.6f;
            result.transform.eulerAngles = Vector3.forward * 90 * -WeaponItem.GetUnit().Core.CoreMovement.FancingDirection;
            result.GetComponentInParent<Rigidbody2D>().gravityScale = 0;
            result.GetComponentInParent<Rigidbody2D>().AddForce(Vector2.right * WeaponItem.GetUnit().Core.CoreMovement.FancingDirection * 200f);
        }
        WeaponItem.SetItemData(null);
    }
}