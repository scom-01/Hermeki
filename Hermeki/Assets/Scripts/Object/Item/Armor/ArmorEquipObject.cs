using UnityEngine;

public class ArmorEquipObject : EquipObject
{
    //public PhotonView PV;
    public Unit targetUnit;
    public ArmorEquipObject(EquipItemData data) : base(data)
    {
    }

    public SpriteRenderer[] SpriteRenderers => GetComponentsInChildren<SpriteRenderer>();
    public override void Interactive(Unit unit)
    {
        Debug.Log($"Armor Equip Interactive {unit.name}");
        unit.ItemManager?.AddArmorItem(Data);
        this.transform.root.gameObject.SetActive(false);

        //PV?.RPC("RPC_Interactive", RpcTarget.AllBuffered, unit.PV.ViewID);
    }
    public override void UnInteractive(Unit unit)
    {
        Debug.Log($"Armor Equip UnInteractive {unit.name}");
    }

    [ContextMenu("SetSpriteRenderer")]
    public override void SetSpriteRenderer()
    {
        if (SpriteRenderers == null || Data.dataSO == null || Data.CurrentDurability == 0)
        {
            this.transform.root.gameObject.SetActive(false);
            return;
        }

        if (!this.transform.root.gameObject.activeSelf)
            this.transform.root.gameObject.SetActive(true);

        int idx = Data.dataSO.CalculateDurability(Data.CurrentDurability);

        for (int i = 0; i < SpriteRenderers.Length; i++)
        {
            SpriteRenderers[i].sprite = Data.dataSO.Sprite[idx].sprites[i];
        }
    }

    #region RPC
    //[PunRPC]
    //private void RPC_Interactive(int idx)
    //{
    //    PhotonView.Find(idx).GetComponent<Unit>()?.ItemManager?.AddArmorItem(Data);
    //    this.transform.root.gameObject.SetActive(false);
    //}
    //[PunRPC]
    //private void RPC_UnInteractive()
    //{

    //}
    #endregion
}
