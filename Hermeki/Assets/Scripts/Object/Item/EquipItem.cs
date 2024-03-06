using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    protected Unit unit;
    protected AnimationEventHandler eventHandler;
    public EquipItemData Data;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        unit = GetComponentInParent<Unit>();
        if (unit != null)
        {
            this.tag = unit.transform.tag;
        }
        eventHandler = unit.GetComponentInChildren<AnimationEventHandler>();
    }
    protected virtual void Start()
    {

    }

    public virtual void DecreaseDurability()
    {
        Data.CurrentDurability--;
    }

    public virtual bool SetItemData(EquipItemData _data)
    {
        if (_data == null || _data.dataSO == null || _data.CurrentDurability == 0)
        {
            return false;
        }
        Data = _data;
        return true;
    }

    public virtual bool DestroyItem()
    {
        if (Data == null || Data.dataSO == null)
        {
            return false;
        }
        SoundManager.Inst.Play(Data.dataSO.BrokenAudioData);
        Debug.Log($"{unit.name} Play {Data.dataSO.name} Audioclip {Data.dataSO.BrokenAudioData}");
        return true;
    }
}
