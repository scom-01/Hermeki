using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayFabManager : Singleton<PlayFabManager>
{
    public string currentPlayFabId;
    public bool isGetData = false;

    private Dictionary<string, int> UserStatisticsDictionary = new Dictionary<string, int>();
    private Dictionary<string, string> UserDataDictionary = new Dictionary<string, string>();

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
        currentPlayFabId = result.PlayFabId;
        
        //유저 데이터 불러오기
        CS_GetUserData();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    #region Statistic 

    private List<StatisticUpdate> _statisticUpdates = new List<StatisticUpdate>();
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
                   //SetStat();
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
               //SetStat();
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
            Debug.Log("Cloud Script (getPlayerStatistic) call succeeded");
            Debug.Log(result.FunctionResult.ToString());
        },
        error =>
            {
                Debug.Log("Cloud Script (getPlayerStatistic) call failed");
                Debug.Log(error.GenerateErrorReport());
            });
    }

    public void CS_SetStatistics(string _key, int _value)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "setPlayerStatistic",
            FunctionParameter = new { statsName = _key, setValue = _value }, // The parameter provided to your function
            GeneratePlayStreamEvent = true,
        },
        result =>
        {
            //값 업데이트
            if (UserStatisticsDictionary.ContainsKey(_key))
            {
                UserStatisticsDictionary[_key] = _value;
            }
            else
            {
                UserStatisticsDictionary.Add(_key, _value);
            }
            Debug.Log("Cloud Script (setPlayerStatistic) call succeeded");            
        },
        error =>
        {
            Debug.Log("Cloud Script (setPlayerStatistic) call failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void CS_SetUserData(string _key, string _value)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "setPlayerData",
            FunctionParameter = new { userDataName = _key, userDataValue = _value }, // The parameter provided to your function
            GeneratePlayStreamEvent = true,
        },
        result =>
        {
            //값 업데이트
            if(UserDataDictionary.ContainsKey(_key))
            {
                UserDataDictionary[_key] = _value;
            }
            else
            {
                UserDataDictionary.Add(_key, _value);
            }
            Debug.Log("Cloud Script (setPlayerData) call succeeded");
        },
        error =>
        {
            Debug.Log("Cloud Script (setPlayerData) call failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    public void CS_SetUserData(string _key, float _value)
    {
        CS_SetUserData(_key, _value.ToString());
    }
    public void CS_SetUserData(string _key, int _value)
    {
        CS_SetUserData(_key, _value.ToString());
    }

    public string GetUserData_Str(string _key)
    {
        string temp = "";
        if (UserDataDictionary.ContainsKey(_key))
        {
            temp = UserDataDictionary[_key];
        }
        return temp;
    }
    public float GetUserData_Float(string _key)
    {
        float temp = -1f;
        if (UserDataDictionary.ContainsKey(_key))
        {
            temp = float.Parse(UserDataDictionary[_key]);
        }
        return temp;
    }
    public int GetUserData_Int(string _key)
    {
        int temp = -1;
        if(UserDataDictionary.ContainsKey(_key))
        {
            temp = int.Parse(UserDataDictionary[_key]);
        }
        return temp;
    }
    public void CS_GetUserData()
    {
        var request = new GetUserDataRequest() { PlayFabId = currentPlayFabId };
        PlayFabClientAPI.GetUserData(request,
            (result) =>
            {
                //초기화
                UserDataDictionary = new Dictionary<string, string>();

                //값 추가
                foreach (var data in result.Data)
                {
                    UserDataDictionary.Add(data.Key, data.Value.Value);
                }
                Debug.Log(UserDataDictionary);

                isGetData = true;
            },
            (error) =>
            {

            });
    }
    #endregion
}
