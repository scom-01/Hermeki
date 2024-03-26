using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

        GetStatAll();

        StartCloudScriptString("helloWorld", "하잉");
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

    public Dictionary<string,int> GetStat()
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

    #region Cloud Script Json
    public object StartCloudScriptString(string FuncName, string FuncParam)
    {
        object temp = null;
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = FuncName,
            FunctionParameter = new { text = FuncParam },
            //PlayStream 표시 여부
            GeneratePlayStreamEvent = true
        },
        (result) =>
        {
            temp = CallbackCloudScriptString(result);
            return;
        },
        (error) =>
        {
            Debug.Log("클라우드 스크립트 호출 오류");
        }) ;
        return temp;
    }

    string CallbackCloudScriptString(ExecuteCloudScriptResult _result)
    {
        JsonObject jsonResult = (JsonObject)_result.FunctionResult;
        jsonResult.TryGetValue("messageValue", out object messageValue);
        return (string)messageValue; 
    }
    #endregion
}
