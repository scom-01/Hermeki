using Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageController : MonoBehaviour
{
    public Transform StartPos;
    public StageEndPoint EndPos;
    private LevelManager LM;
    public TilemapRenderer TM_Renderer;
    public PolygonCollider2D PC2D;
    public StageItemController SI_Controller;
    private void Awake()
    {
        LM = GetComponentInParent<LevelManager>();
        TM_Renderer = GetComponentInChildren<TilemapRenderer>();
        TM_Renderer.enabled = false;
        SI_Controller = GetComponentInChildren<StageItemController>();
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
        if (SI_Controller != null)
        {
            SI_Controller.ClearObject();
        }
        return true;
    }
    public bool EnterEndpos(Player player)
    {
        if (player == null)
            return false;
        if (LM.GoNextStage(player))
        {
            ControllTM(false);
        }


        if (this.TryGetComponent(out ActionEventHandler _action))
        {
            _action.EndAction();
        }

        ResetStage();

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
        if (TM_Renderer == null)
            return false;

        TM_Renderer.enabled = isTrue;
        return true;
    }
}
