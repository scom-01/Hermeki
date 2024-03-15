using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    /*Param*/
    public static float GravityPower = 5f;

    public static float GV_SystemDeltatime = 0.02f;
    public static float GV_CharacterDeltatime = 0.02f;

    public const string SFX_ContainerTagName = "SoundContainer";
    public const string VFX_ContainerTagName = "EffectContainer";

    #region PlayerPrefs Data
    public const string StageName = "StageName";
    public const string NextStageName = "NextStage";
    public const string RebindsKey = "rebinds";

    public static GameObject Base_Projectile = Resources.Load<GameObject>("Prefabs/Projectiles/Base_Projectile");
    public static GameObject Base_EffectPooling = Resources.Load<GameObject>("Prefabs/Effects/Base_EffectPooling");
    public static GameObject Base_ProjectilePooling = Resources.Load<GameObject>("Prefabs/Projectiles/Base_ProjectilePooling");
    public static GameObject Base_DmgTxtPooling = Resources.Load<GameObject>("Prefabs/Effects/Base_DmgTxtPooling");
    public static GameObject Base_AttackMessageBox= Resources.Load<GameObject>("Prefabs/Effects/AttackMessageBox");
    public static GameObject Base_SoundPooling = Resources.Load<GameObject>("Prefabs/Sounds/Base_SoundPooling");
    public static GameObject Base_SpawnGoodsItem = Resources.Load<GameObject>("Prefabs/Item/Goods/GoodsItem");
    public static GameObject Base_GoodsChunk = Resources.Load<GameObject>("Prefabs/Item/Goods/GoodsChunk");

    public static Sprite Base_SPUM_Cloth = Resources.Load<Sprite>("SPUM/SPUM_Sprites/Items/2_Cloth/Cloth_9");
    //public static GoodsSO Goods_Gold = Resources.Load<GoodsSO>("Prefabs/Item/Goods/Gold");
    //public static GoodsSO Goods_Fire = Resources.Load<GoodsSO>("Prefabs/Item/Goods/FireGoods");
    //public static GoodsSO Goods_Water = Resources.Load<GoodsSO>("Prefabs/Item/Goods/WaterGoods");
    //public static GoodsSO Goods_Wind = Resources.Load<GoodsSO>("Prefabs/Item/Goods/WindGoods");
    //public static GoodsSO Goods_Earth = Resources.Load<GoodsSO>("Prefabs/Item/Goods/EarthGoods");
    //public static GoodsSO Goods_Hammer = Resources.Load<GoodsSO>("Prefabs/Item/Goods/HammerShards");
    /// <summary>
    /// AudioMixer의 SFX Parameter값과 일치해야함
    /// </summary>
    public const string SFX_Vol = "SFX_Vol";
    /// <summary>
    /// AudioMixer의 BGM Parameter값과 일치해야함
    /// </summary>
    public const string BGM_Vol = "BGM_Vol";
    public const string Quality = "Quality";
    public const string Language = "Language";

    public const string UI_Table = "Localization/StringTables/UI_Table";
    #endregion

    public const string FadeInCutScene = "TimeLine/FadeIn";
    public const string FadeOutCutScene = "TimeLine/FadeOut";
    public const string TitleFadeOutCutScene = "TimeLine/TitleFadeOut";

    public const string GoldCount = "GoldCount";
    public const string ElementalCount = "ElementalCount";


    #region UI Prefab
    public static GameObject DamageTextPrefab = Resources.Load<GameObject>("ScriptPath/UI/Prefabs/DamageText");
    public static GameObject CriticalDamageTextPrefab = Resources.Load<GameObject>("ScriptPath/UI/Prefabs/CriticalDamageText");
    #endregion

    #region AudioSources
    public static AudioClip CriticalHit_SFX = Resources.Load<AudioClip>("ScriptPath/UI/Sounds/CriticalHit_sfx");
    #endregion

    #region UI Icon Path
    public const string Icon_UI_Path = "ScriptPath/UI/Goods";
    public const string Icon_Gold_Path = "GoldIcon";
    public const string Icon_FireGoods_Path = "FireGoods";
    public const string Icon_WaterGoods_Path = "WaterGoods";
    public const string Icon_EarthGoods_Path = "EarthGoods";
    public const string Icon_WindGoods_Path = "WindGoods";

    public const string Sprites_UI_Path = "ScriptPath/UI/Sprites";
    public const string Symbol_Normal_Path = "Symbol_Normal";
    public const string Symbol_Gold_Path = "Symbol_Gold";
    public const string Symbol_Fire_Path = "Symbol_Fire";
    public const string Symbol_Water_Path = "Symbol_Water";
    public const string Symbol_Earth_Path = "Symbol_Earth";
    public const string Symbol_Wind_Path = "Symbol_Wind";
    public const string Symbol_Hammer_Path = "Symbol_Hammer";

    public const string StatsSprites_UI_Path = "Stats_Sheet";
    public static Sprite Box = Resources.Load<Sprite>("Sprites/Box");
    #endregion

    #region UI Sound Path
    public const string Sounds_UI_Path = "ScriptPath/UI/Sounds/";
    public const string Reforging_Success = "Reforging_Success";
    public const string Reforging_Failure = "Reforging_Failure";
    #endregion

    #region DB_Path
    public const int MaxSceneIdx = 50;
    public static int[] DefaultUnlockItem = { 0, 1, 2, 3, 4, 5, 6, };
    #endregion

    public static int Gold_Inflation = 250;
    public static int ReRoll_Inflation = 60;
    public static int Bless_Inflation = 1000;
    public static int BlessingStats_Inflation = 5;

    /// <summary>
    /// Elemetal 속성 공격에 따른 추가 데미지 퍼센트
    /// ex) 0.3f일때 Weak Elemental에 대한 추가 데미지는 30%이다.
    /// </summary>
    public static float E_WeakPer = 0.3f;

    /// <summary>
    /// Enemy의 Size에 따른 추가 데미지 퍼센트
    /// Player의 사이즈는 Medium으로 정의하여 계산
    /// ex) 0.3f일때 Small Enemy에 대한 추가 데미지는 30%이다.
    /// ex) 0.3f일때 Big Enemy에 대해 받는 추가 데미지는 30%이다.
    /// </summary>
    public static float Enemy_Size_WeakPer = 0.1f;
    /*~Param*/

    //Check Animator Parameters
    public static bool ContainParam(Animator _Anim, string _paramName)
    {
        foreach (AnimatorControllerParameter param in _Anim.parameters)
        {
            if (param.name == _paramName) return true;
        }
        return false;
    }
}

