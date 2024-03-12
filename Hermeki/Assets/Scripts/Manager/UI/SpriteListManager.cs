using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct SPUM_Sprite
{
    public string SpritePath;
    public Sprite Sprite;
}
public enum SPUM_SpriteSettingType
{
    Hair = 0,
    Cloth = 1,
    Back = 2,
}

public class SpriteListManager : MonoBehaviour
{
    public List<SPUM_Sprite> SpriteItemList = new List<SPUM_Sprite>();
    public Image Img;
    public SPUM_SpriteSettingType Type;
    public SPUM_SpriteList spum_spritelist;
    [SerializeField]
    int CurrIdx = 0;

    private void Start()
    {
        SetSpumSpriteList(Type);
        SetSprite();
    }
    public void Left()
    {
        if (SpriteItemList == null || SpriteItemList.Count == 0 || Img == null)
            return;
        CurrIdx--;
        if (CurrIdx < 0)
            CurrIdx = SpriteItemList.Count - 1;
        if (!SetSpumSpriteList(Type))
        {
            return;
        }
        if (!SetSprite())
        {
            return;
        }
    }

    public void Right()
    {
        if (SpriteItemList == null || SpriteItemList.Count == 0 || Img == null)
            return;
        CurrIdx++;
        if (CurrIdx > SpriteItemList.Count - 1)
            CurrIdx = 0;
        if (!SetSpumSpriteList(Type))
        {
            return;
        }
        if (!SetSprite())
        {
            return;
        }
    }

    public bool SetSpumSpriteList(SPUM_SpriteSettingType _type)
    {
        if (spum_spritelist == null)
            return false;

        switch (_type)
        {
            case SPUM_SpriteSettingType.Hair:
                spum_spritelist.SetHair(SpriteItemList[CurrIdx].SpritePath);
                break;
            case SPUM_SpriteSettingType.Cloth:
                spum_spritelist.SetBody(SpriteItemList[CurrIdx].SpritePath);
                break;
            case SPUM_SpriteSettingType.Back:
                spum_spritelist.SetBack(SpriteItemList[CurrIdx].SpritePath);
                break;
            default:
                break;
        }
        return true;
    }

    public bool SetSprite()
    {
        if (Img == null || SpriteItemList[CurrIdx].Sprite == null)
            return false;

        Img.sprite = SpriteItemList[CurrIdx].Sprite;
        return true;
    }
}
