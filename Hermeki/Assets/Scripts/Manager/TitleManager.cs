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

    private void Start()
    {
        PlayFabManager.Inst?.TryLogin();
	}

    public void MoveToSinglePlay()
    {
        //if (!PlayFabManager.Inst.isGetData)
        //    return;

        SceneManager.LoadSceneAsync("Ingame");
    }

    public void SetLoadingPanel(bool _isactive) => LoadingPanel.SetActive(_isactive);
}
