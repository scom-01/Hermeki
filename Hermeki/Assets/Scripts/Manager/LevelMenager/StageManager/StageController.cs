using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageController : MonoBehaviour
{
    public Transform StartPos;
    public StageEndPoint EndPos;
    private LevelManager LM;
    public List<Grid> GirdList = new List<Grid>();
    public int CurrLevel = 0;
    public PolygonCollider2D PC2D;
    public StageObjectController SO_Controller;
    private void Awake()
    {
        LM = GetComponentInParent<LevelManager>();
        GirdList = GetComponentsInChildren<Grid>().ToList();
        foreach (var _grid in GirdList)
        {
            _grid.enabled = false;
        }
        //TM_RendererList.enabled = false;
        SO_Controller = GetComponentInChildren<StageObjectController>();
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
        if (GirdList?.Count == 0 || GirdList.Count <= CurrLevel || GirdList[CurrLevel] == null)
            return false;

        GirdList[CurrLevel].enabled = isTrue;
        return true;
    }
}
