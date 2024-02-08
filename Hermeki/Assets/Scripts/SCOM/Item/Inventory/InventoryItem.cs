using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, ISelectHandler
{
    /// <summary>
    /// 인벤토리 Data
    /// </summary>
    public StatsItemSO StatsItemData
    {
        get => statsItemData;
        set
        {
            statsItemData = value;
            if (value == null)
            {

            }
            else
            {
                OnChangeStatItemData?.Invoke();
            }
            Init();
        }
    }
    [SerializeField] private StatsItemSO statsItemData;
    public bool isSelect;
    public int Index;
    public Action OnChangeStatItemData;
    /// <summary>
    /// 아이템 아이콘 이미지
    /// </summary>
    public Image iconImg;
    /// <summary>
    /// 방향키로 움직일 버튼
    /// </summary>
    public Button Btn;
    [ContextMenu("Set Index")]
    public void SetIndex()
    {
        var Parent = this.transform.parent;
        var MaxIndex = Parent.GetComponentInParent<InventoryItems>().MaxIndex;
        var MaxRow = Parent.GetComponentInParent<InventoryItems>().MaxRow;
        var items = Parent.gameObject.GetComponentsInChildren<InventoryItem>();
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != this)
            {
                continue;
            }

            Index = i;
            var nav = this.GetComponent<Button>().navigation;
            if (i - 1 >= 0)
            {
                nav.selectOnLeft = items[i - 1].GetComponent<Button>();
            }
            //가장 왼쪽, 가장 아래있는 버튼
            else
            {
                nav.selectOnLeft = items[MaxIndex - 1].GetComponent<Button>();
            }

            if (i + 1 < items.Length)
            {
                nav.selectOnRight = items[i + 1].GetComponent<Button>();
            }

            if (i - (MaxIndex / MaxRow) >= 0)
            {
                nav.selectOnUp = items[i - (MaxIndex / MaxRow)].GetComponent<Button>();
            }
            if (i + (MaxIndex / MaxRow) < items.Length)
            {
                nav.selectOnDown = items[i + (MaxIndex / MaxRow)].GetComponent<Button>();
            }
            items[i].GetComponent<Button>().navigation = nav;
            return;
        }
    }

    //Index를 현재 Select된 아이템 Index로 설정
    public void CurrentItem()
    {
        if (this.GetComponentInParent<InventoryItems>() == null)
        {
            return;
        }

        this.GetComponentInParent<InventoryItems>().CurrentSelectItemIndex = Index;
    }

    public void Init()
    {
        if (statsItemData != null)
        {
            if(iconImg != null)
            {
                iconImg.enabled = true;
                iconImg.sprite = statsItemData.itemData.ItemSprite;
                iconImg.color = new Color(iconImg.color.r, iconImg.color.g, iconImg.color.b, 1f);
            }
        }
        else
        {
            if(iconImg != null)
            {
                iconImg.enabled = false;
                iconImg.sprite = null;
                iconImg.color = new Color(iconImg.color.r, iconImg.color.g, iconImg.color.b, 0);
            }
        }
    }

    //OnSelect
    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        if (eventData.selectedObject == this.gameObject)
        {
            Debug.Log($"{this.gameObject.name} selected");
            CurrentItem();
        }
    }
}