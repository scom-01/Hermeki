using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DownloadManager : MonoBehaviour
{
    /// <summary>
    /// 전체 다운로드 파일 진행 표시 슬라이더
    /// </summary>
    [Header("UI")]
    public GameObject UpdateMessage;
    /// <summary>
    /// 전체 업데이트 용량 표시 텍스트
    /// </summary>
    public TMP_Text UpdateSizeText;
    [Space(15)]
    public Slider AlldownloadSilder;
    /// <summary>
    /// 전체 다운로드 진행률 표시 텍스트
    /// </summary>
    public TMP_Text AlldownloadPerText;
    /// <summary>
    /// 전체 다운로드 진행률 표시 텍스트
    /// </summary>
    public TMP_Text AllsizeInfoText;
    [Space(15)]
    /// <summary>
    /// 현재 다운로드 파일 진행 표시 슬라이더
    /// </summary>
    public Slider CurrdownloadSlider;
    /// <summary>
    /// 현재 다운로드 파일 진행률 표시 텍스트
    /// </summary>
    public TMP_Text CurrdownloadPerText;
    /// <summary>
    /// 현재 다운로드 파일 정보 표시 텍스트
    /// </summary>
    public TMP_Text CurrsizeInfoText;

    [Header("Label")]
    public AssetLabelReference defaultLabel;
    public AssetLabelReference RemoteLabel;

    /// <summary>
    /// 전체 패치 사이즈
    /// </summary>
    private long patchSize;
    private Dictionary<string, long> patchMap = new Dictionary<string, long>();

    private void Awake()
    {
        AlldownloadSilder.value = 0f;
        CurrdownloadSlider.value = 0f;
        AlldownloadPerText.text = string.Format("00.0 %");
        AllsizeInfoText.text = string.Format("0 Bytes / 0 Bytes");
        CurrdownloadPerText.text = string.Format("00.0 %");
        CurrsizeInfoText.text = string.Format("0 Bytes / 0 Bytes");
    }
    private void Start()
    {
        UpdateMessage.SetActive(false);
        UpdateSizeText.gameObject.SetActive(false);
        StartCoroutine(CheckUpdateFiles());
    }

    #region Check
    IEnumerator CheckUpdateFiles()
    {
        var labels = new List<string> { defaultLabel.labelString, RemoteLabel.labelString };

        patchSize = default;

        foreach (var label in labels)
        {
            //byte로 받아옴
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle;
            patchSize += handle.Result;
        }

        //업데이트할 파일 존재
        if (patchSize > decimal.Zero)
        {
            UpdateMessage.SetActive(true);
            UpdateSizeText.gameObject.SetActive(true);
            UpdateSizeText.text = GetFileSize(patchSize);
            AllsizeInfoText.text = GetFileSize(patchSize);

        }
        else
        {
            CurrdownloadPerText.text = string.Format("100.0 %");
            CurrdownloadSlider.value = 1f;

            AlldownloadPerText.text = string.Format("100.0 %");
            AlldownloadSilder.value = 1f;

            SceneManager.LoadSceneAsync(0);
            yield return null;
        }
    }

    private string GetFileSize(long byteCount)
    {
        string size = "0 Bytes";
        if (byteCount >= 1073741824.0)
        {
            size = string.Format($"{(byteCount / 1073741824).ToString("0.00")} GB");
        }
        else if (byteCount >= 1048576.0)
        {
            size = string.Format($"{(byteCount / 1048576).ToString("0.00")} MB");
        }
        else if (byteCount >= 1024.0)
        {
            size = string.Format($"{(byteCount / 1024.0).ToString("0.00")} KB");
        }
        else if (byteCount > 0 && byteCount < 1024.0)
        {
            size = string.Format($"{byteCount} Bytes");
        }
        return size;
    }
    #endregion

    #region Download
    public void Download()
    {
        StartCoroutine(PatchFiles());
    }
    IEnumerator PatchFiles()
    {
        var labels = new List<string> { defaultLabel.labelString, RemoteLabel.labelString };

        foreach (var label in labels)
        {
            //byte로 받아옴
            var handle = Addressables.GetDownloadSizeAsync(label);

            yield return handle;

            if (handle.Result != decimal.Zero)
            {
                StartCoroutine(DownLoadLabel(label));
            }
            //Addressables.Release(handle);
        }

        yield return CheckDownLoad();
    }
    IEnumerator DownLoadLabel(string label)
    {
        patchMap.Add(label, 0);

        //label 다운로드
        var handle = Addressables.DownloadDependenciesAsync(label, false);

        while (!handle.IsDone)
        {
            patchMap[label] = handle.GetDownloadStatus().DownloadedBytes;
            yield return new WaitForEndOfFrame();

            //진행사항 보고
            CurrdownloadSlider.value = handle.GetDownloadStatus().Percent;
            CurrdownloadPerText.text = string.Format($"{(CurrdownloadSlider.value * 100f).ToString("00.00")} %");
            CurrsizeInfoText.text = string.Format($"{GetFileSize(handle.GetDownloadStatus().DownloadedBytes)} / {GetFileSize(handle.GetDownloadStatus().TotalBytes)}");
        }

        //다운로드 중인 label 번들 파일의 총 크기
        patchMap[label] = handle.GetDownloadStatus().TotalBytes;
        Addressables.Release(handle);
    }

    IEnumerator CheckDownLoad()
    {
        long total = 0;
        AlldownloadPerText.text = "0 %";
        while (true)
        {
            //patchMap(labels)의 모든 파일 사이즈 합
            total += patchMap.Sum(tmp => tmp.Value);

            if (patchSize > decimal.Zero)
            {
                //진행사항 보고
                AlldownloadSilder.value = total / patchSize;
                AlldownloadPerText.text = string.Format($"{(AlldownloadSilder.value * 100f).ToString("00.00")} %");
                AllsizeInfoText.text = string.Format($"{GetFileSize(total)} / {GetFileSize(patchSize)}");

                //다운로드 완료
                if (total == patchSize)
                {
                    break;
                }
                total = 0;
                yield return new WaitForEndOfFrame();
            }
            else
            {
                AlldownloadPerText.text = "100.0 %";
                break;
            }
        }

        SceneManager.LoadSceneAsync(0);
    }
    #endregion
}
