using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BuffSystem : MonoBehaviour
{
    /// <summary>
    /// 액티브 버프 리스트
    /// </summary>
    public List<Buff> ActiveBuffList = new List<Buff>();
    private Unit _unit;

    /// <summary>
    /// 패시브 버프 리스트
    /// </summary>
    public List<Buff> PassiveBuffList = new List<Buff>();

    /// <summary>
    /// 패시브 버프는 특정한 상황일 때 부여되므로 그 진행상황을 저장하여 진생상황이 소실되지 않도록 함
    /// </summary>
    public List<Buff> Old_PassiveBuffList = new List<Buff>();
    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }

    private void Update()
    {
        ActiveBuffListUpdate();
    }

    private void ActiveBuffListUpdate()
    {
        if (ActiveBuffList == null)
        {
            ActiveBuffList = new List<Buff>();
            return;
        }
        if (ActiveBuffList.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < ActiveBuffList.Count;)
        {
            //버프 지속 시간이 무한 일시 넘어감
            if (ActiveBuffList[i].buffItemSO.BuffData.DurationTime == 999f)
            {
                i++;
                continue;
            }

            //현재 플레이 시간 > 버프 시작 시간 + 버프 지속 시간
            if (GameManager.Inst?.PlayTime > ActiveBuffList[i].startTime + ActiveBuffList[i].buffItemSO.BuffData.DurationTime)
            {
                Debug.Log($"Time = {Time.time}");
                Debug.Log($"GlobalTime = {GameManager.Inst?.PlayTime}");
                //버프 스탯 제거
                _unit.Core.CoreUnitStats.RemoveStat(ActiveBuffList[i].buffItemSO.StatsData);

                //버프 스택
                ActiveBuffList[i].CurrBuffCount--;

                //버프 스택이 0일시 버프 제거
                if (ActiveBuffList[i].CurrBuffCount < 1)
                {
                    RemoveBuff(ActiveBuffList[i]);
                }
                //버프 스택이 1 이상 일 시 버프 시간 시간 += 버프 지속 시간
                else
                {
                    ActiveBuffList[i].startTime += ActiveBuffList[i].buffItemSO.BuffData.DurationTime;
                }

                if (ActiveBuffList.Count <= 0)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            i++;
        }

    }

    /// <summary>
    /// 현재 적용중인 버프 중 BuffItemSO가 동일한 버프를 반환
    /// </summary>
    /// <param name="buffItemSO"></param>
    /// <returns></returns>
    public Buff FindCurrentBuff(BuffItemSO buffItemSO)
    {
        switch (buffItemSO.BuffData.BuffType)
        {
            case EVENT_BUFF_TYPE.Active:
                for (int i = 0; i < ActiveBuffList.Count; i++)
                {
                    if (ActiveBuffList[i].buffItemSO == buffItemSO)
                    {
                        return ActiveBuffList[i];
                    }
                }
                break;
            case EVENT_BUFF_TYPE.Passive:
                for (int i = 0; i < PassiveBuffList.Count; i++)
                {
                    if (PassiveBuffList[i].buffItemSO == buffItemSO)
                    {
                        return PassiveBuffList[i];
                    }
                }
                break;
            default:
                break;
        }

        return null;
    }

    /// <summary>
    /// 과거 적용했던 버프 중 BuffItemSO가 동일한 버프를 반환
    /// </summary>
    /// <param name="buffItemSO"></param>
    /// <returns></returns>
    public Buff FindOldBuff(BuffItemSO buffItemSO)
    {
        switch (buffItemSO.BuffData.BuffType)
        {
            case EVENT_BUFF_TYPE.Active:
                break;
            case EVENT_BUFF_TYPE.Passive:
                for (int i = 0; i < Old_PassiveBuffList.Count; i++)
                {
                    if (Old_PassiveBuffList[i].buffItemSO == buffItemSO)
                    {
                        return Old_PassiveBuffList[i];
                    }
                }
                break;
            default:
                break;
        }

        return null;
    }

    /// <summary>
    /// 저장된 버프의 스탯을 적용하기 위함
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool AddBuffStats(Buff buff, int count)
    {
        if (buff == null)
            return false;

        if (count > 0)
        {
            _unit.Core.CoreUnitStats.AddStat(buff.buffItemSO.StatsData * count);
        }

        //버프대상이 플레이어라면 BuffPanel UI에 버프 표시
        if (_unit.GetType() == typeof(Player))
        {
            //GameManager.Inst?.MainUI?.MainPanel?.BuffPanelSystem.BuffPanelAdd(buff);
        }

        return true;
    }

    public bool AddBuff(Buff _buff)
    {
        if (_buff == null)
            return false;

        switch (_buff.buffItemSO.BuffData.BuffType)
        {
            case EVENT_BUFF_TYPE.Active:
                foreach (var tempbuff in ActiveBuffList)
                    if (tempbuff == _buff) return false;
                ActiveBuffList.Add(_buff);
                return true;
            case EVENT_BUFF_TYPE.Passive:
                foreach (var tempbuff in PassiveBuffList)
                    if (tempbuff == _buff) return false;
                PassiveBuffList.Add(_buff);
                return true;
            default:
                foreach (var tempbuff in ActiveBuffList)
                    if (tempbuff == _buff) return false;
                ActiveBuffList.Add(_buff);
                return true;
        }
    }

    public bool IncreaseBuff(Buff _buff)
    {
        _buff.startTime = GameManager.Inst.PlayTime;

        switch (_buff.buffItemSO.BuffData.BuffType)
        {
            case EVENT_BUFF_TYPE.Active:
                return CalculateAddBuff(ActiveBuffList, _buff);
            case EVENT_BUFF_TYPE.Passive:
                return CalculateAddBuff(PassiveBuffList, _buff);
            default:
                return CalculateAddBuff(ActiveBuffList, _buff);
        }
    }

    private bool CalculateAddBuff(List<Buff> _bufflist, Buff _buff)
    {
        //이미 버프 동일한 버프가 있을 때
        for (int i = 0; i < _bufflist.Count; i++)
        {
            if (_bufflist[i].buffItemSO != _buff.buffItemSO)
                continue;

            //버프 시작 시간 초기화
            if (_bufflist[i].buffItemSO.BuffData.isBuffInit)
            {
                _bufflist[i].startTime = GameManager.Inst.PlayTime;
            }

            //중복 X
            if (!_bufflist[i].buffItemSO.BuffData.isOverlap)
            {
                return false;
            }

            //중복 최대치 
            if (_bufflist[i].CurrBuffCount >= _bufflist[i].buffItemSO.BuffData.BuffCountMax)
            {
                return false;
            }

            _bufflist[i].CurrBuffCount++;
            _unit.Core.CoreUnitStats.AddStat(_bufflist[i].buffItemSO.StatsData);
            if (_buff.buffItemSO.Health != 0.0f)
            {
                _unit.Core.CoreUnitStats.IncreaseHealth(_bufflist[i].buffItemSO.Health);
            }

            return true;
        }

        //버프 리스트에 버프 추가
        _bufflist?.Add(_buff);
        //버프 스택 증가
        _buff.CurrBuffCount++;
        //버프대상이 플레이어라면 BuffPanel UI에 버프 표시
        if (_unit.GetType() == typeof(Player))
        {
            //GameManager.Inst?.MainUI?.MainPanel?.BuffPanelSystem.BuffPanelAdd(_buff);
        }
        //버프 스탯 증가
        _unit.Core.CoreUnitStats.AddStat(_buff.buffItemSO.StatsData);

        //버프 체력 증가
        if (_buff.buffItemSO.Health != 0.0f)
        {
            _unit.Core.CoreUnitStats.IncreaseHealth(_buff.buffItemSO.Health);
        }
        return true;
    }

    /// <summary>
    /// 버프 제거
    /// </summary>
    /// <param name="buff">제거 될 버프</param>
    /// <returns></returns>
    public bool RemoveBuff(Buff buff)
    {
        if (buff.CurrBuffCount > 0)
        {
            //버프 스탯 제거
            _unit.Core.CoreUnitStats.RemoveStat(buff.buffItemSO.StatsData * buff.CurrBuffCount);
        }

        switch (buff.buffItemSO.BuffData.BuffType)
        {
            case EVENT_BUFF_TYPE.Active:
                return CalculateRemoveBuff(ActiveBuffList, buff);
            case EVENT_BUFF_TYPE.Passive:
                if (!Old_PassiveBuffList.Contains(buff))
                {
                    //패시브 버프 저장
                    Old_PassiveBuffList.Add(buff);
                }

                return CalculateRemoveBuff(PassiveBuffList, buff);
            default:
                return CalculateRemoveBuff(ActiveBuffList, buff);
        }
    }

    private bool CalculateRemoveBuff(List<Buff> _bufflist, Buff _buff)
    {
        if (_unit.GetType() == typeof(Player))
        {
            GameManager.Inst?.MainUI?.MainPanel?.BuffPanelSystem.BuffPanelRemove(_buff);
        }

        _bufflist.Remove(_buff);
        return true;
    }

    public void SetBuff()
    {
        for (int i = 0; i < ActiveBuffList.Count; i++)
        {
            //버프의 CurrBuffCount에 맞는 스탯을 재적용
            AddBuffStats(ActiveBuffList[i], ActiveBuffList[i].CurrBuffCount);
        }

        for (int i = 0; i < PassiveBuffList.Count; i++)
        {
            //버프의 CurrBuffCount에 맞는 스탯을 재적용
            AddBuffStats(PassiveBuffList[i], PassiveBuffList[i].CurrBuffCount);
        }
    }
}
