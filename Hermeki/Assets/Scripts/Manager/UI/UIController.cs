using System.Collections.Generic;
using UnityEngine;

public enum UI_Type
{
    Horizontal = 0,
    Vertical = 1,
    Grid = 2,
}

public enum UI_RectTransform_Anchor
{
    Top_Left = 0,
    Top_Center = 1,
    Top_Right = 2,
    Top_Stretch = 4,
    Middle_Left = 5,
    Middle_Center = 6,
    Middle_Right = 7,
    Middle_Stretch = 8,
    Bottom_Left = 9,
    Bottom_Center = 10,
    Bottom_Right = 11,
    Bottom_Stretch = 12,
    Stretch_Left = 13,
    Stretch_Center = 14,
    Stretch_Right = 15,
    Stretch_Stretch = 16,
}
//UI에 관한 동작을 EventSystem에서 손쉽게 호출하도록 함수들을 모아놓은 클래스
public class UIController : MonoBehaviour
{
    public UI_Type type;

    public bool isInfinity;
    public bool isAutoSize;

    public float Spacing;

    public int MaxCount;
    public GameObject Base_ContentObject;

    public List<RectTransform> childList = new List<RectTransform>();
    [HideInInspector]
    public List<EquipItemData> DataList = new List<EquipItemData>();

    private RectTransform rectTransform;
    private Canvas canvas;
    private void Awake()
    {
        canvas = this.GetComponentInParent<Canvas>();
        rectTransform = this.GetComponent<RectTransform>();
    }
    private void Start()
    {
        SetUp();
    }

    private void OnEnable()
    {
        if (isAutoSize)
        {
            this.GetComponentInParent<UI_Canvas>().OnEnableAction -= this.AutoSize;
            this.GetComponentInParent<UI_Canvas>().OnEnableAction += this.AutoSize;
        }
    }
    private void OnDisable()
    {
        if (this.GetComponentInParent<UI_Canvas>() != null)
        {
            this.GetComponentInParent<UI_Canvas>().OnEnableAction -= this.AutoSize;
        }
    }

    public bool SetDataList(List<EquipItemData> _dataList)
    {
        Clear();
        if (_dataList?.Count == 0)
        {
            DataList = new List<EquipItemData>();
            return false;
        }
        DataList = _dataList;
        foreach (var item in DataList)
        {
            AddChild(item);
        }
        return true;
    }

    [ContextMenu("SetUp")]
    public void SetUp()
    {
        if (rectTransform == null)
            rectTransform = this.GetComponent<RectTransform>();
        childList = new List<RectTransform>();
        foreach (RectTransform child in transform)
        {
            if (childList.Count >= MaxCount)
                break;
            childList.Add(child);
        }
    }

    public bool AddChild(EquipItemData _data)
    {
        if (_data?.dataSO == null)
            return false;
        switch (_data.dataSO.ItemType)
        {
            case Item_Type.Armor:
                AddChild(Base_ContentObject)?.GetComponent<InventoryItemUI>()?.SetData(_data);
                break;
            case Item_Type.Weapon:
                AddChild(Base_ContentObject)?.GetComponent<InventoryItemUI>()?.SetData(_data);
                break;
            case Item_Type.Rune:
                AddChild(Base_ContentObject)?.GetComponent<InventoryItemUI>()?.SetData(_data);
                break;
            default:
                break;
        }
        return true;
    }
    public GameObject AddChild(GameObject obj)
    {
        if (childList.Count >= MaxCount)
            return null;
        GameObject result = Instantiate(obj, this.transform);
        childList.Add(result.GetComponent<RectTransform>());
        return result;
    }

    public GameObject RemoveChild(EquipItemData _data)
    {
        if (_data?.dataSO == null || DataList?.Count == 0)
            return null;

        if (DataList.Contains(_data))
        {
            DataList.Remove(_data);
        }
        return null;
    }

    /// <summary>
    /// 아이템 교체
    /// </summary>
    /// <param name="_currData">현재 장착(사용) 중인 아이템</param>
    /// <param name="_oldData">교체 아이템</param>
    /// <returns></returns>
    public EquipItemData ChangeEquipData(EquipItemData _currData, EquipItemData _oldData)
    {
        if (RemoveChild(_oldData) == null)
        {
            return null;
        }
        if (AddChild(_currData) == null)
        {

        }
        return null;
    }
    public bool AddChild(Transform obj)
    {
        if (childList.Count >= MaxCount)
            return false;
        childList.Add(obj.GetComponent<RectTransform>());
        obj.parent = this.transform;
        return true;
    }

    /// <summary>
    /// Controller 하위 오브젝트 삭제
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < childList.Count; i++)
        {
            if (childList[i] != null)
                Destroy(childList[i].gameObject);
            childList[i] = null;
        }
        childList = new List<RectTransform>();
    }

    [ContextMenu("AutoSize")]
    public void AutoSize()
    {
        //SetUp();
        if (childList?.Count == 0)
        {
            switch (type)
            {
                case UI_Type.Horizontal:
                    rectTransform.sizeDelta = new Vector2(Spacing, rectTransform.sizeDelta.y);
                    break;
                case UI_Type.Vertical:
                    rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Spacing);
                    break;
                case UI_Type.Grid:
                    break;
                default:
                    break;
            }
            return;
        }

        float width = 0, height = 0;
        //자식 오브젝트의 총 height
        for (int i = 0; i < childList.Count; i++)
        {
            height += childList[i].sizeDelta.y;
        }
        //자식 오브젝트의 총 width
        for (int i = 0; i < childList.Count; i++)
        {
            width += childList[i].sizeDelta.x;
        }

        switch (type)
        {
            case UI_Type.Horizontal:
                rectTransform.sizeDelta = new Vector2(width + (childList.Count - 1) * Spacing, rectTransform.sizeDelta.y);
                break;
            case UI_Type.Vertical:
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height + (childList.Count - 1) * Spacing);

                float temp = 0;
                for (int i = 0; i < childList.Count; i++)
                {
                    //Anchors Top Center
                    childList[i].anchorMin = new Vector2(0, 1);
                    childList[i].anchorMax = new Vector2(1f, 1);
                    childList[i].anchoredPosition = new Vector2(0, -(childList[i].sizeDelta.y / 2) - (i * Spacing) - temp);
                    temp += childList[i].sizeDelta.y;
                }
                break;
            case UI_Type.Grid:
                break;
            default:
                break;
        }
    }
}
