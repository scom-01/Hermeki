using Cinemachine;
using UnityEngine;

public class StageController : MonoBehaviour, ILevelManagerObserver
{
    public Transform StartPos;
    public StageEndPoint EndPos;
    private LevelManager LM;
    public Grid Gird;
    /// <summary>
    /// 스테이지 레벨
    /// </summary>
    public int CurrLevel = 0;
    /// <summary>
    /// 스테이지 idx
    /// </summary>
    public int CurrIdx = 0;
    public PolygonCollider2D PC2D;
    public CompositeCollider2D CpC2D;
    [HideInInspector] public StageObjectController SO_Controller;    
    private void Awake()
    {
        LM = this.GetComponentInParent<LevelManager>();
        Gird = this.GetComponentInChildren<Grid>();
        Gird.enabled = false;
        SO_Controller = GetComponentInChildren<StageObjectController>();

        LM.registerObserver(this);
    }
    public bool ResetStage()
    {
        if (!ControllTM(false))
        {
            return false;
        }

        if (this.TryGetComponent(out ActionEventHandler _action))
        {
            _action.EndAction();
        }
        if (SO_Controller != null)
        {
            SO_Controller.ClearObject();
        }
        return true;
    }
    public bool EnterEndpos(Player player)
    {
        if (player == null)
            return false;
        
        LM.GoNextStage(player);
        //if(LM.CurrStageIdx < LM.MaxLevel)
        //{
        //    //ResetStage();            
        //}

        if (this.TryGetComponent(out ActionEventHandler _action))
        {
            _action.EndAction();
        }

        return true;
    }

    public bool StartStage()
    {
        if (!ControllTM(true))
        {
            return false;
        }

        LM.VirtualCamera.GetComponent<CinemachineConfiner2D>().enabled = true;
        if (PC2D != null)
        {
            LM.VirtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = PC2D;
        }
        if(CpC2D!=null)
        {
            LM.VirtualCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = CpC2D;
        }

        if (this.TryGetComponent(out ActionEventHandler _action))
        {
            _action.StartAction();
        }
        return true;
    }
    private bool ControllTM(bool isTrue)
    {
        if (Gird == null)
            return false;

        Gird.enabled = isTrue;
        return true;
    }

    #region interface
    

    public void UpdateStageLevel(int _value)
    {
        CurrLevel = _value;
    }

    public void UpdateStageIdx(int _value)
    {
        if(CurrIdx == _value)
        {
            this.gameObject.SetActive(true);
            StartStage();
            return;
        }
        this.gameObject.SetActive(false);
        ResetStage();        
    }
    #endregion
}
