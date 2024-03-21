using System.Collections.Generic;
using UnityEngine;

public class AutoSize : MonoBehaviour
{
    [Header("AutoSize")]
    [SerializeField] private UI_Type type;
    [SerializeField] private UI_RectTransform_Anchor Anchortype;
    [SerializeField] private float Spacing;
    [SerializeField] private int MaxCount;
    [SerializeField] private List<RectTransform> childList = new List<RectTransform>();
    [SerializeField] private float padding_Left;
    [SerializeField] private float padding_Right;
    [SerializeField] private float padding_Top;
    [SerializeField] private float padding_Bottom;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
    }
    public void SetChild()
    {
        childList = new List<RectTransform>();
        foreach (RectTransform child in transform)
        {
            if (childList.Count >= MaxCount)
                break;
            childList.Add(child);
        }
    }

    public void AddChild(RectTransform _transform)
    {
        childList.Add(_transform);
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
    public void ReSize()
    {
        if (childList?.Count == 0)
        {
            switch (type)
            {
                case UI_Type.Horizontal:
                    rectTransform.sizeDelta = new Vector2(Spacing, rectTransform.sizeDelta.y);
                    break;
                case UI_Type.Vertical:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
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
                float temp = padding_Top;
                for (int i = 0; i < childList.Count; i++)
                {
                    //Anchors Top Center
                    childList[i].anchorMin = new Vector2(0, 1);
                    childList[i].anchorMax = new Vector2(1f, 1);
                    childList[i].anchoredPosition = new Vector2(0, -(childList[i].sizeDelta.y / 2) - (i * Spacing) - temp);
                    childList[i].sizeDelta = new Vector2(rectTransform.sizeDelta.x, childList[i].sizeDelta.y);
                    childList[i].offsetMin = new Vector2(padding_Left, childList[i].offsetMin.y);
                    childList[i].offsetMax = new Vector2(-padding_Right, childList[i].offsetMax.y);
                    temp += childList[i].sizeDelta.y;
                }
                temp += (childList.Count - 1) * Spacing;
                temp += padding_Bottom;
                //Anchors Stretch
                SetAnchor();
                rectTransform.offsetMin = new Vector2(0, rectTransform.offsetMin.y);
                rectTransform.offsetMax = new Vector2(0, rectTransform.offsetMax.y);
                rectTransform.sizeDelta = new Vector2(0, temp);
                break;
            case UI_Type.Grid:
                break;
            default:
                break;
        }
    }

    private void SetAnchor()
    {
        switch (Anchortype)
        {
            case UI_RectTransform_Anchor.Top_Left:
                rectTransform.anchorMin = Vector2.up;
                rectTransform.anchorMax = Vector2.up;
                break;
            case UI_RectTransform_Anchor.Top_Center:
                rectTransform.anchorMin = new Vector2(0.5f, 1);
                rectTransform.anchorMax = new Vector2(0.5f, 1);
                break;
            case UI_RectTransform_Anchor.Top_Right:
                rectTransform.anchorMin = new Vector2(1, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                break;
            case UI_RectTransform_Anchor.Top_Stretch:
                rectTransform.anchorMin = new Vector2(0, 1);
                rectTransform.anchorMax = new Vector2(1, 1);
                break;
            case UI_RectTransform_Anchor.Middle_Left:
                rectTransform.anchorMin = new Vector2(0, 0.5f);
                rectTransform.anchorMax = new Vector2(0, 0.5f);
                break;
            case UI_RectTransform_Anchor.Middle_Center:
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                break;
            case UI_RectTransform_Anchor.Middle_Right:
                rectTransform.anchorMin = new Vector2(1f, 0.5f);
                rectTransform.anchorMax = new Vector2(1f, 0.5f);
                break;
            case UI_RectTransform_Anchor.Middle_Stretch:
                rectTransform.anchorMin = new Vector2(0f, 0.5f);
                rectTransform.anchorMax = new Vector2(1f, 0.5f);
                break;
            case UI_RectTransform_Anchor.Bottom_Left:
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(0f, 0f);
                break;
            case UI_RectTransform_Anchor.Bottom_Center:
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0f);
                break;
            case UI_RectTransform_Anchor.Bottom_Right:
                rectTransform.anchorMin = new Vector2(1f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 0f);
                break;
            case UI_RectTransform_Anchor.Bottom_Stretch:
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 0f);
                break;
            case UI_RectTransform_Anchor.Stretch_Left:
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                break;
            case UI_RectTransform_Anchor.Stretch_Center:

                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                break;
            case UI_RectTransform_Anchor.Stretch_Right:
                rectTransform.anchorMin = new Vector2(1f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
                break;
            case UI_RectTransform_Anchor.Stretch_Stretch:
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
                break;
            default:
                break;
        }
    }
}
