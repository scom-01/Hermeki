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
        if (result.GetComponentInChildren<EquipObject>() != null)
        {
            result.GetComponentInParent<ProjectileObject>().SetUnit(WeaponItem.GetUnit());
            result.GetComponentInParent<ProjectileObject>().InIt();
        }
        WeaponItem.SetItemData(null);
    }
}