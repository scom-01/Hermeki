﻿using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using PlayFab.MultiplayerModels;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Inst
    {
        get
        {
            if (_Inst == null)
            {
                _Inst = FindObjectOfType(typeof(PlayFabManager)) as PlayFabManager;
                if (_Inst == null)
                {
                    Debug.Log("no Singleton obj");
                }
                else
                {
                    DontDestroyOnLoad(_Inst.gameObject);
                }
            }
            return _Inst;
        }
    }
    private static PlayFabManager _Inst = null;


    private void Awake()
    {
        if (_Inst)
        {
            var managers = Resources.FindObjectsOfTypeAll(typeof(PlayFabManager));
            for (int i = 0; i < managers.Length; i++)
            {
                Debug.Log($"{managers[i]} = {i}");
                if (i > 0)
                {
                    Destroy(managers[i].GameObject());
                }
            }
            return;
        }

        _Inst = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "42";
        }
        var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    #region Statistic 

    private List<StatisticUpdate> _statisticUpdates = new List<StatisticUpdate>();
    public void SetStat(string key, int value)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {

            }
        },
        (result) => { print("값 저장 성공"); },
        (error) => { print("값 저장 실패"); });
    }
    public void SetStat()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate{StatisticName = "Wizard",Value = 0},
                new StatisticUpdate{StatisticName = "Beggar",Value = 0},
            }
        },
        (result) => { print("값 저장 성공"); },
        (error) => { print("값 저장 실패"); });
    }

    [ContextMenu("Get ClearTime")]
    public int GetClearTime()
    {
        int value = -1;
        PlayFabClientAPI.GetPlayerStatistics(
           new GetPlayerStatisticsRequest(),
           (result) =>
           {
               if (result.Statistics.Count <= 0)
               {
                   print("값이 설정되어 있지 않습니다.");
                   return;
               }

               for (int i = 0; i < result.Statistics.Count; i++)
               {
                   if (result.Statistics[i].StatisticName == "ClearTime")
                   {

                       print("값 불러오기 성공");
                       value = result.Statistics[i].Value;
                       Debug.Log(value);
                       return;
                   }
               }
               print("값이 설정되어 있지 않습니다.");
               return;
           },
           (error) =>
           {
               print("값 불러오기 실패");
           });
        return value;
    }

    /// <summary>
    /// result = -1일 때 값이 존재하지않음
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetStat(string key)
    {
        int value = -1;
        PlayFabClientAPI.GetPlayerStatistics(
           new GetPlayerStatisticsRequest(),
           (result) =>
           {
               if (result.Statistics.Count <= 0)
               {
                   print("값이 설정되어 있지 않습니다.");
                   return;
               }

               for (int i = 0; i < result.Statistics.Count; i++)
               {
                   if (result.Statistics[i].StatisticName == key)
                   {

                       print("값 불러오기 성공");
                       value = result.Statistics[i].Value;
                       return;
                   }
               }
               print("값이 설정되어 있지 않습니다.");
               return;
           },
           (error) =>
           {
               print("값 불러오기 실패");
           });
        return value;
    }

    public Dictionary<string, int> GetStat()
    {
        Dictionary<string, int> temp = new Dictionary<string, int>();
        PlayFabClientAPI.GetPlayerStatistics(
           new GetPlayerStatisticsRequest(),
           (result) =>
           {
               if (result.Statistics.Count <= 0)
               {
                   print("값이 설정되어 있지 않습니다.");
                   return;
               }
               print("값 불러오기 성공");
               foreach (var eachStat in result.Statistics)
               {
                   temp.Add(eachStat.StatisticName, eachStat.Value);
                   //print($"{eachStat.StatisticName} : {eachStat.Value}");
               }
           },
           (error) =>
           {
               print("값 불러오기 실패");
           });
        return temp;
    }
    public void GetStatAll()
    {
        PlayFabClientAPI.GetPlayerStatistics(
           new GetPlayerStatisticsRequest(),
           (result) =>
           {
               if (result.Statistics.Count <= 0)
               {
                   print("값이 설정되어 있지 않습니다.");
                   SetStat();
                   return;
               }
               print("값 불러오기 성공");
               foreach (var eachStat in result.Statistics)
               {
                   print($"{eachStat.StatisticName} : {eachStat.Value}");
               }
           },
           (error) =>
           {
               print("값 불러오기 실패");
               SetStat();
           });
    }
    #endregion

    //클라우드 스크립트는 비동기 처리되며 그 값을 저장 및 불러오고 바로 적용되지 않는다는 걸 인지해야함
    #region Cloud Script Json
    public void CS_GetStatistics(string Name)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "getPlayerStatistic",
            FunctionParameter = new { Name = Name }, // The parameter provided to your function
            GeneratePlayStreamEvent = true,
        },
        result =>
        {
            Debug.Log("Cloud Script call succeeded");
            Debug.Log(result.FunctionResult.ToString());
        },
        error =>
            {
                Debug.Log("Cloud Script call failed");
                Debug.Log(error.GenerateErrorReport());
            });
    }

    public void CS_SetStatistics(string Name, int _value)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "setPlayerStatistic",
            FunctionParameter = new { statsName = Name, setValue = _value }, // The parameter provided to your function
            GeneratePlayStreamEvent = true,
        },
        result =>
        {
            Debug.Log("Cloud Script call succeeded");
            CS_GetStatistics(Name);
        },
        error =>
        {
            Debug.Log("Cloud Script call failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    #endregion
}
