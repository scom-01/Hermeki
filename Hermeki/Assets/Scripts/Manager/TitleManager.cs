using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public bool isGetData { get; private set; }
    public GameObject LoadingPanel;
    private void Awake()
    {
        if (LoadingPanel != null)
        {
            LoadingPanel.SetActive(true);
        }
    }
    private void Update()
    {
        if (!isGetData && PlayFabManager.Inst != null && PlayFabManager.Inst.isGetData)
        {
            //값 불러옴
            isGetData = true;
            if (LoadingPanel != null) LoadingPanel.SetActive(false);
        }
    }
    public void MoveToSinglePlay()
    {
        if (!isGetData)
            return;

        SceneManager.LoadSceneAsync("Ingame");
    }
}
