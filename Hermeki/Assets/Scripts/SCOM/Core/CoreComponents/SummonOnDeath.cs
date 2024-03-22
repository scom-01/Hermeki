using SCOM.CoreSystem;
using UnityEngine.AddressableAssets;

public class SummonOnDeath : Death
{
    public AssetReferenceGameObject UnitObject;
    public override void Die()
    {
        base.Die();
        SummonUnit(UnitObject);
    }    
    private void SummonUnit(AssetReferenceGameObject UnitObject)
    {
        if (UnitObject == null)
            return;

        GameManager.Inst?.LevelManager?.CurrStage()?.SO_Controller?.SpawnUnit(UnitObject, core.CoreCollisionSenses.UnitCenterPos);
    }
}
