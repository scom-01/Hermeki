using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Canvas : MonoBehaviour
{
    public Canvas Canvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = this.GetComponent<Canvas>();
            }
            return _canvas;
        }
        set
        {
            _canvas = value;
        }
    }
    Canvas _canvas;

    public ContentSizeFitter SizeFitter
    {
        get
        {
            if (_sizeFitter == null)
            {
                _sizeFitter = this.GetComponent<ContentSizeFitter>();
            }
            return _sizeFitter;
        }
        set
        {
            _sizeFitter = value;
        }
    }
    ContentSizeFitter _sizeFitter;

    public UIController UI_Controller;

    public event Action OnEnableAction;
    public event Action OnDisableAction;

    public InventoryState State;

    private InventoryTable _inventoryTable;

    [Header("AutoSize")]
    [SerializeField] private AutoSize autoSize;
    private RectTransform rectTransform;
    private void Awake()
    {
        UI_Controller = this.GetComponentInChildren<UIController>();
        _inventoryTable = this.GetComponentInParent<InventoryTable>();
        autoSize = this.GetComponent<AutoSize>();
    }
    public virtual void Canvas_Enable()
    {
        SetContent();
        OnEnableAction?.Invoke();
        ContentReSizing();
        Canvas.enabled = true;
    }
    public virtual void Canvas_Disable()
    {
        OnDisableAction?.Invoke();
        Canvas.enabled = false;
    }

    public void AddContent(GameObject obj)
    {
        if (UI_Controller == null)
            return;

        if (UI_Controller.AddChild(obj))
        {
            ContentReSizing();
        }
    }

    public void SetContent()
    {
        if (UI_Controller == null || _inventoryTable == null)
        {
            return;
        }
        switch (State)
        {
            case InventoryState.Table:
                break;
            case InventoryState.EquipItem:
                //장착 가능한 아이템 리스트
                UI_Controller.SetDataList(_inventoryTable.ResearchEquipable());
                break;
            case InventoryState.ChoiceHand:
                break;
            case InventoryState.Rune:
                //룬 아이템 리스트
                UI_Controller.SetDataList(_inventoryTable.ResearchRune());
                break;
            case InventoryState.Enchant:
                UI_Controller.SetDataList(_inventoryTable.ResearchEnchantable());
                break;
            case InventoryState.Close:
                break;
            default:
                break;
        }
    }

    private void ContentReSizing()
    {
        if (autoSize != null)
        {
            autoSize.SetChild();
            autoSize.ReSize();
        }
        //SizeFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
        //LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }
}
