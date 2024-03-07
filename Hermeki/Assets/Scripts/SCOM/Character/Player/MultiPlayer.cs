using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayer : Player,IPunObservable
{
    [Header("Photon")]
    public PhotonView PV;

    #region Unity Callback Func
    protected override void Awake()
    {
        isMulti = true;
        base.Awake();
        PV = GetComponent<PhotonView>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();        
    }

    protected override void FixedUpdate()
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
    }
    #endregion
}
