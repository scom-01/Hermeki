using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryItems : MonoBehaviour
{
    public List<InventoryItem> Items
    {
        get
        {
            if(items.Count == 0)
            {
                items = this.GetComponentsInChildren<InventoryItem>().ToList();
            }
            return items;
        }
    }
    private List<InventoryItem> items = new List<InventoryItem>();

    public GameObject InventoryItemPrefab;
    [Min(1)]
    public int MaxIndex;
    [Min(1)]
    public int MaxRow;

    /// <summary>
    /// 현재 선택된 아이템
    /// </summary>
    public InventoryItem CurrentSelectItem
    {
        get => currentSelectItem;
        set
        {
            if (currentSelectItem != null)
                currentSelectItem.isSelect = false;

            currentSelectItem = value;
            currentSelectItem.isSelect = true;
        }
    }
    [SerializeField]private InventoryItem currentSelectItem;

    /// <summary>
    /// 현재 선택된 아이템 인덱스
    /// </summary>
    public int CurrentSelectItemIndex
    {
        get => currentSelectItemIndex;
        set
        {
            if(items.Count  != 0)
            {
                currentSelectItemIndex = Mathf.Clamp(value, 0, items.Count - 1);
            }
            else
            {
                currentSelectItemIndex = 0;
            }
            CurrentSelectItem = Items[currentSelectItemIndex];
            //GameManager.Inst.SubUI.InventorySubUI.InventoryDescript.SetDescript();
        }
    }
    private int currentSelectItemIndex = 0;

    private void Init()
    {
        if (InventoryItemPrefab == null)
            return;

        if (this.GetComponentsInChildren<InventoryItem>().Length == 0)
        {            
            for (int i = 0; i < MaxIndex; i++)
            {                
                var item = Instantiate(InventoryItemPrefab, this.transform).GetComponent<InventoryItem>();
                Items.Add(item);
                item.Index = i;
            }
        }
        else if(this.GetComponentsInChildren<InventoryItem>().Length != MaxIndex)
        {
            for (int i = this.GetComponentsInChildren<InventoryItem>().Length; i < MaxIndex; i++)
            {
                var item = Instantiate(InventoryItemPrefab, this.transform).GetComponent<InventoryItem>();
                Items.Add(item);
                item.Index = i;
            }
        }
    }

    public void AddItem(StatsItemSO StatsItem)
    {        
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].StatsItemData != null)
            {
                if (Items[i].StatsItemData == StatsItem)
                {
                    break;
                }
                continue;
            }
            else
            {   
                Debug.LogWarning($"{Items[i].name}.StatsItemData is Null");
                Items[i].StatsItemData = StatsItem;
                CurrentSelectItemIndex = i;
                Items[i].Index = CurrentSelectItemIndex;
                break;
            }
        }
    }
    public void RemoveItem(StatsItemSO StatsItem)
    {
        foreach(var item in Items)
        {
            if(item.StatsItemData == StatsItem)
            {
                for(int i = item.Index; i < Items.Count-1; i++)
                {
                    Items[i].StatsItemData = Items[i + 1].StatsItemData;
                }
                Items.Last().StatsItemData = null;
                return;
            }
        }
        //CurrentSelectItem.StatsItemData = null;
        //CurrentSelectItemIndex--;
    }
}