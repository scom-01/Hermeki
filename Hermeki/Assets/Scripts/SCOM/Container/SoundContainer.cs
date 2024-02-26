using System.Collections.Generic;
using SCOM;
using UnityEngine;

public class SoundContainer : MonoBehaviour
{
    public GameObject Base_SoundPooling;
    public List<SoundPooling> SoundPoolingList = new List<SoundPooling>();
    public SoundPooling CheckObject(AudioData _Sfx)
    {
        if(SoundPoolingList.Count == 0)
        {
            var obj = AddObject(_Sfx).GetComponent<SoundPooling>();
            return obj;
        }
        for (int i = 0; i < SoundPoolingList.Count; i++)
        {
            if (SoundPoolingList[i].SoundObject.Clip == _Sfx.Clip)
            {
                return SoundPoolingList[i];
            }
        }
        var newObj = AddObject(_Sfx).GetComponent<SoundPooling>();
        return newObj;
    }

    public GameObject AddObject(AudioData _Sfx)
    {
        if (Base_SoundPooling == null)
        {
            Base_SoundPooling = GlobalValue.Base_SoundPooling;
        }
        var SoundPool = Instantiate(Base_SoundPooling, this.transform);
        SoundPool.GetComponent<SoundPooling>().Init(_Sfx);
        SoundPoolingList.Add(SoundPool.GetComponent<SoundPooling>());
        return SoundPool;
    }
}
