using Cinemachine;
using SCOM;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("----Manager----")]
    //public ItemManager IM;
    //public SpawnManager SPM;
    public LightProbeProxyVolume StageVolume;
    public List<Transform> MasterManagerList;

    public List<StageController> StageList = new List<StageController>();
    public int CurrStageIdx = 0;
    [Header("----Scene----")]
    /// <summary>
    /// Stage 시작 시 Stage이름 Fade
    /// </summary>
    [SerializeField] private GameObject SceneNameFade;
    public string CurrStageName
    {
        get
        {
            return SceneManager.GetActiveScene().name;
        }
    }

    [Tooltip("true면 다음 씬은 무조건 현재씬 idx +1 ")]
    public bool isGotoNext = true;

    [Tooltip("현재 스테이지 단계")]
    [Min(1)]
    public int StageLevel = 1;
    public int CurrStageNumber
    {
        get
        {
            if (GameManager.Inst.SceneNameList.Count > 0)
            {
                return GameManager.Inst.SceneNameList.FindIndex(str => str.Equals(SceneManager.GetActiveScene().name));
            }
            return 0;
        }
    }
    public int NextStageNumber
    {
        get
        {
            if (isGotoNext)
            {
                return CurrStageNumber + 1;
            }
            return m_NextStageNumber;
        }
    }

    public int m_NextStageNumber;
    public UI_State Start_UIState;

    [HideInInspector] public bool isStageClear = false;
    /// <summary>
    /// Stage 클리어 시 생성
    /// </summary>
    public GameObject EndingCutSceneDirector;

    [Header("----Cam----")]
    public Camera Cam;
    [HideInInspector] public CinemachineVirtualCamera CVC;
    public float Cam_Distance;
    public EffectContainer EffectContainer
    {
        get
        {
            if (effectContainer == null)
            {
                effectContainer = GameObject.FindGameObjectWithTag(GlobalValue.VFX_ContainerTagName).GetComponent<EffectContainer>();
                if (effectContainer == null)
                {
                    effectContainer = GameObject.FindGameObjectWithTag(GlobalValue.VFX_ContainerTagName).AddComponent<EffectContainer>();
                }
            }
            return effectContainer;
        }
    }
    private EffectContainer effectContainer;
    public SoundContainer SFXContainer
    {
        get
        {
            if (_sfxContainer == null)
            {
                _sfxContainer = GameObject.FindGameObjectWithTag(GlobalValue.SFX_ContainerTagName).GetComponent<SoundContainer>();
                if (_sfxContainer == null)
                {
                    _sfxContainer = GameObject.FindGameObjectWithTag(GlobalValue.SFX_ContainerTagName).AddComponent<SoundContainer>();
                }
            }
            return _sfxContainer;
        }
    }
    private SoundContainer _sfxContainer;

    [Header("----Player----")]

    public GameObject PlayerPrefab;
    private GameObject playerGO;
    [HideInInspector] public Player player;
    /// <summary>
    /// Player SpawnPoint
    /// </summary>
    public Transform SpawnPoint;

    /// <summary>
    /// Next Scene Portal Point
    /// </summary>
    public Transform EndPoint;

    [Header("----Item----")]
    [Tooltip("아이템 스폰 위치")]
    /// <summary>
    /// Item Spawn Point
    /// </summary>
    public Transform ItemSpawnPoint;
    [Tooltip("스폰되는 아이템 수")]
    /// <summary>
    /// Item Spawn Amount
    /// </summary>
    public int ItemSpawnAmount;
    [Tooltip("스폰되는 아이템들의 x좌표 간격")]
    /// <summary>
    /// Item Spawn Interval
    /// </summary>
    public float ItemSpawnInterval;

    [Header("----Sounds----")]
    public AudioSource BGM;
    public AudioData BGM_source;

    private void Awake()
    {
        Debug.LogWarning($"Stage Awake {CurrStageName}");
        if (GameManager.Inst)
        {
            GameManager.Inst.StageManager = this;
        }
        //playerGO = Instantiate(PlayerPrefab, SpawnPoint);
        //player = playerGO.GetComponent<Player>();

        //if (Cam == null)
        //    Cam = this.GetComponentsInChildren<Camera>()[0];

        //var FadeIn = Resources.Load<GameObject>(GlobalValue.FadeInCutScene);
        //if (FadeIn != null)
        //{
        //    Instantiate(FadeIn);
        //}
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        //CVC = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        StageList = GetComponentsInChildren<StageController>().ToList();
        if (CVC != null)
        {
            CVC.Follow = player.transform;
            CVC.m_Lens.OrthographicSize = (Cam_Distance <= 0) ? 5 : Cam_Distance;
        }
        isStageClear = false;
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {

        //if (StageVolume == null)
        //    StageVolume = this.GetComponentInChildren<LightProbeProxyVolume>();

        //if (SpawnPoint == null)
        //    SpawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();

        //if (BGM == null)
        //    BGM = GameObject.Find("BGM").GetComponent<AudioSource>();
        //if (BGM_source.Clip != null)
        //{
        //    BGM.clip = BGM_source.Clip;
        //    BGM.volume = BGM_source.Volume;
        //    BGM.loop = true;
        //    BGM.playOnAwake = false;
        //    BGM.Play();
        //}

        //GameManager.Inst.InputHandler.ChangeCurrentActionMap(InputEnum.GamePlay, false);

        if (player != null)
        {
            player.Inventory?.ItemExeOnMoveMap(player);
        }
    }

    public void StartStage()
    {
        if (StageList.Count == 0)
            return;

        if(!StageList[0].StartStage())
        {
            return;
        }

        return;
    }
    public bool GoNextStage(Player player)
    {
        if (player == null)
            return false;

        CurrStageIdx++;
        player.transform.position = StageList[CurrStageIdx].StartPos.position;
        return true;
    }
}
