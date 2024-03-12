using Photon.Pun;
using UnityEngine;

public class MultiPlayer : Player, IPunObservable
{
    public PhotonView PV;
    #region Unity Callback Func
    protected override void Awake()
    {
        isMulti = true;
        base.Awake();
        PV = GetComponent<PhotonView>();
        currPos = transform.position;
    }

    protected override void Start()
    {
        base.Start();
    }

    private Vector3 currPos = Vector3.zero;
    protected override void Update()
    {
        if (PV.IsMine)
        {
            base.Update();
        }
        else
        {
            if (10 < (transform.position - currPos).magnitude)
            {
                transform.position = currPos;
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10);
            }
        }
        //else if ((transform.position - currPos).sqrMagnitude >= 100)
        //{
        //    transform.position = currPos;
        //}
        //else
        //{
        //    transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 10);
        //}
    }

    protected override void FixedUpdate()
    {
        if (PV.IsMine)
        {
            base.FixedUpdate();
        }
    }
    [PunRPC]
    void RPCUpdate()
    {
        base.Update();
    }
    [PunRPC]
    void RPCFixedUpdate()
    {
        base.FixedUpdate();
    }
    #endregion

    #region Other Func

    private void AnimationTrigger() => FSM.CurrentState.AnimationTrigger();

    #endregion

    #region Override

    public override void DieEffect()
    {
        base.DieEffect();
    }

    public override void HitEffect()
    {
        base.HitEffect();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            //Debug.Log($"{this.name} pos = {transform.position}, serverpos = {currPos}");
        }
    }
    #endregion
}
