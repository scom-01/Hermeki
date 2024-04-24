using UnityEngine;

public class StageLevelController : MonoBehaviour, ILevelManagerObserver
{
    [Tooltip("StageLevelController를 보유한 객체가 표시되고자하는 스테이지 레벨 ex) 2 -> 스테이지 레벨이 2일때 SetActive(true)")]
    /// <summary>
    /// StageLevelController를 보유한 객체가 표시되고자하는 스테이지 레벨 ex) 2 -> 스테이지 레벨이 2일때 SetActive(true)
    /// </summary>
    [SerializeField]
    private int StageLevel;
    [Tooltip("true = StageLevel이 LevelManager의 레벨보다 낮으면 같지 않아도 활성화")]
    /// <summary>
    /// true = StageLevel이 LevelManager의 레벨보다 낮으면 같지 않아도 활성화
    /// </summary>
    [SerializeField]
    private bool isOverlap;
    private LevelManager _levelManager;
    //public StageLevelController(StageController _stageController)
    //{
    //    this.stageController = _stageController;
    //    stageController.registerObserver(this);
    //}

    private void Awake()
    {
        _levelManager = this.GetComponentInParent<LevelManager>();
        _levelManager.registerObserver(this);
    }

    public void UpdateStageLevel(int _value)
    {
        if (isOverlap)
        {
            if (StageLevel <= _value)
            {
                this.gameObject.SetActive(true);
                return;
            }
            this.gameObject.SetActive(false);
            return;
        }

        if (StageLevel == _value)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
