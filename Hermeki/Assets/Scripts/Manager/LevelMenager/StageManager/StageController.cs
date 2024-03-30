using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageController : MonoBehaviour, ILevelManagerObserver
{
    public Transform StartPos;
    public StageEndPoint EndPos;
    private LevelManager LM;
    public Grid Gird;
    /// <summary>
    /// 스테이지 레벨에 따른 다른 맵 적용을 위한 리스트
    /// </summary>
    public List<GameObject> LevelObjectList = new List<GameObject>();   
    /// <summary>
    /// 스테이지 레벨
    /// </summary>
    public int CurrLevel = 0;
    public PolygonCollider2D PC2D;
    [HideInInspector] public StageObjectController SO_Controller;
    
    private LevelManager _levelManager;
    private void Awake()
    {
        LM = this.GetComponentInParent<LevelManager>();
        Gird = this.GetComponentInChildren<Grid>();
        Gird.enabled = false;
        SO_Controller = GetComponentInChildren<StageObjectController>();

        _levelManager = this.GetComponentInParent<LevelManager>();
        _levelManager.registerObserver(this);
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
        if (LM.GoNextStage(player))
        {
            ResetStage();
        }


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
    #endregion
}
