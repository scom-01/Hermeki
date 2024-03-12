using UnityEngine;

public class WeaponEquipObject : EquipObject
{
    //public PhotonView PV;
    public Unit targetUnit;
    public WeaponEquipObject(EquipItemData data) : base(data)
    {
    }

    public ItemObject itemObject => this.transform.root.GetComponent<ItemObject>();
    public override void Interactive(Unit unit)
    {
        Debug.Log($"Weapon Equip Interactive {unit.name}");
        unit.ItemManager?.AddWeaponItem(Data);
        this.transform.root.gameObject.SetActive(false);
        //PV?.RPC("RPC_Interactive", RpcTarget.AllBuffered, unit.PV.ViewID);
    }

    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Weapon Equip UnInteractive {unit.name}");        
        //PV?.RPC("RPC_UnInteractive", RpcTarget.AllBuffered);
    }
    [ContextMenu("SetSpriteRenderer")]
    public override void SetSpriteRenderer()
    {
        if (itemObject == null || Data.dataSO == null || Data.CurrentDurability == 0)
        {
            this.transform.root.gameObject.SetActive(false);
            return;
        }

        if (!this.transform.root.gameObject.activeSelf)
            this.transform.root.gameObject.SetActive(true);

        itemObject.SetSpriteRenderer(Data);
    }

    #region RPC
    //[PunRPC]
    //private void RPC_Interactive(int idx)
    //{
    //    PhotonView.Find(idx).GetComponent<Unit>()?.ItemManager?.AddWeaponItem(Data);
    //    this.transform.root.gameObject.SetActive(false);
    //}
    //[PunRPC]
    //private void RPC_UnInteractive()
    //{

    //}

    #endregion
}
