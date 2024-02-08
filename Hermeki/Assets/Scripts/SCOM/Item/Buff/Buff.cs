using SCOM.CoreSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  
public class Buff 
{
    public BuffItemSO buffItemSO;

    //Script Object와의 차이점
    public int CurrBuffCount = 0;
    public float startTime = 0f;

    public Buff(BuffItemSO buffItemSO = null, int currBuffCount = 0, float startTime = 0f)
    {
        this.buffItemSO = buffItemSO;
        CurrBuffCount = currBuffCount;
        this.startTime = startTime;
    }

    public static Buff BuffSystemAddBuff(Unit unit, BuffItemSO data)
    {
        if (data == null || unit == null)
            return null;

        Buff buff = new Buff(data);
        if (unit.Core.CoreSoundEffect && data.InitEffectData.AcquiredSFX.Clip != null)
            unit.Core.CoreSoundEffect.AudioSpawn(data.InitEffectData.AcquiredSFX);

        if (unit.GetComponent<BuffSystem>()?.IncreaseBuff(buff) != null)
        {
            return buff;
        }

        return null;
    }

    public static Buff BuffSystemRemoveBuff(Unit unit, BuffItemSO data)
    {
        if (data == null || unit == null)
            return null;

        Buff buff = new Buff(data);
        if (unit.GetComponent<BuffSystem>()?.IncreaseBuff(buff) != null) 
        {
            return buff;
        }

        return null;
    }
}