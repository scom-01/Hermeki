using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class CurrItem : MonoBehaviour, IUI_Select
{
    [SerializeField] private Image IconImg;
    [SerializeField] private LocalizeStringEvent Local_Name;
    [SerializeField] private LocalizeStringEvent Local_Descript;
    [SerializeField] private LocalizeStringEvent Local_Event_Name;
    [SerializeField] private LocalizeStringEvent Local_Event_Descript;
    [SerializeField] private LocalizeStringEvent Local_ItemLevel;
    [SerializeField] private TMP_Text Text_Stat;
    private StatsItemSO m_itemSO;

    private void Start()
    {
        if (m_itemSO == null)
        {
            ClearText();
        }
    }

    public void Select(GameObject go)
    {
        if (go == null)
            return;

        if (go.GetComponent<InventoryItem>() == null)
            return;

        m_itemSO = go.GetComponent<InventoryItem>().StatsItemData;

        if (m_itemSO == null)
        {
            ClearText();
        }
        else
        {
            if (IconImg != null)
            {
                IconImg.enabled = true;
                IconImg.sprite = m_itemSO.itemData.ItemSprite;
            }

            if (Local_Descript != null)
                Local_Descript.StringReference.SetReference("Item_Table", m_itemSO.itemData.ItemDescriptionLocal.TableEntryReference);

            if (Local_Name != null)
                Local_Name.StringReference.SetReference("Item_Table", m_itemSO.itemData.ItemNameLocal.TableEntryReference);

            if (Local_Event_Name != null)
            {
                if (m_itemSO.EventNameLocal.TableEntryReference.KeyId != 0) Local_Event_Name.StringReference.SetReference("ItemEvent_Table", m_itemSO.EventNameLocal.TableEntryReference);
                else Local_Event_Name.StringReference.SetReference("Item_Table", "Empty");
            }

            if (Local_Event_Descript != null)
            {
                if (m_itemSO.EventDescriptionLocal.TableEntryReference.KeyId != 0) Local_Event_Descript.StringReference.SetReference("ItemEvent_Descript_Table", m_itemSO.EventDescriptionLocal.TableEntryReference);
                else Local_Event_Descript.StringReference.SetReference("Item_Table", "Empty");
            }

            if (Local_ItemLevel != null)
            {
                Local_ItemLevel.StringReference.SetReference("UI_Table", m_itemSO.itemData.ItemLevel.ToString());
            }

            if (Text_Stat == null)
                return;

            if (m_itemSO.StatsItems.Count > 0)
            {
                Text_Stat.text = m_itemSO.StatsData_Descripts;
            }
            else
            {
                Text_Stat.text = "";
            }
        }

    }

    private void ClearText()
    {
        if (IconImg != null)
            IconImg.enabled = false;

        if (Local_Descript != null)
            Local_Descript.StringReference.SetReference("Item_Table", "Empty");

        if (Local_Name != null)
            Local_Name.StringReference.SetReference("Item_Table", "Empty");

        if (Local_Event_Name != null)
            Local_Event_Name.StringReference.SetReference("Item_Table", "Empty");

        if (Local_Event_Descript != null)
            Local_Event_Descript.StringReference.SetReference("Item_Table", "Empty");

        if (Local_ItemLevel != null)
            Local_ItemLevel.StringReference.SetReference("Item_Table", "Empty");

        if (Text_Stat != null)
            Text_Stat.text = "";
    }
}
