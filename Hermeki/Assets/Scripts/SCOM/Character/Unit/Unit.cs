using SCOM.CoreSystem;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isMulti = false;
    #region Component
    public Core Core
    {
        get
        {
            if (core == null)
                core = this.GetComponentInChildren<Core>();
            return core;
        }
        private set
        {
            core = value;
        }
    }
    private Core core;
    public UnitFSM FSM { get; private set; }
    public Animator Anim { get; private set; }
    public Animator[] Anims { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public CapsuleCollider2D CC2D
    {
        get
        {
            if (cc2d == null)
            {
                cc2d = this.GetComponent<CapsuleCollider2D>();
                if (cc2d != null)
                {
                    cc2d.sharedMaterial = UnitData.UnitCC2DMaterial;
                    return cc2d;
                }
                cc2d = this.GameObject().AddComponent<CapsuleCollider2D>();
                cc2d.sharedMaterial = UnitData.UnitCC2DMaterial;
                cc2d.offset = UnitData.standCC2DOffset;
                cc2d.size = UnitData.standCC2DSize;
            }
            return cc2d;
        }
        private set
        {
            cc2d = value;
        }
    }

    private CapsuleCollider2D cc2d;

    public SpriteRenderer SR { get; private set; }
    [Tooltip("Map에 표시될 SpriteRenderer")]
    public SpriteRenderer MapSR;

    public ItemManager ItemManager;
    public Inventory Inventory { get; private set; }

    [HideInInspector] public Transform RespawnPoint;

    public UnitData UnitData;

    public bool IsAlive = true;
    private DamageFlash[] DamageFlash;

    /// <summary>
    /// 절대 CC 면역값
    /// </summary>
    [SerializeField] private bool isFixed_CC_Immunity = false;
    /// <summary>
    /// 절대 피격 면역값
    /// </summary>
    [SerializeField] private bool isFixed_Hit_Immunity = false;

    /// <summary>
    /// KnockBack, JumpPad 등 외부요인으로 움직이는지 판별하는 요소
    /// </summary>
    [HideInInspector] public bool isFixedMovement = false;

    /// <summary>
    /// 면역값
    /// </summary>
    public bool isCC_immunity
    {
        get
        {
            if (isFixed_CC_Immunity)
            {
                _isCCimmunity = true;
            }
            return _isCCimmunity;
        }
        set
        {
            _isCCimmunity = value;
            if (isFixed_CC_Immunity)
            {
                _isCCimmunity = true;
            }
        }
    }

    private bool _isCCimmunity;

    /// <summary>
    /// 타겟 유닛
    /// </summary>
    [HideInInspector]

    private Unit TargetUnit;

    #endregion

    #region Set variable Func
    public void Set_Fixed_CC_Immunity(bool value) => isFixed_CC_Immunity = value;
    public void Set_Fixed_Hit_Immunity(bool value) => isFixed_Hit_Immunity = value;
    public bool Get_Fixed_CC_Immunity { get => isFixed_CC_Immunity; }
    public bool Get_Fixed_Hit_Immunity { get => isFixed_Hit_Immunity; }
    #endregion

    #region Unity Callback Func
    protected virtual void Awake()
    {
        InitSetting();
        DamageFlash = GetComponentsInChildren<DamageFlash>();
    }

    private void InitSetting()
    {
        FSM = new UnitFSM();

        if (UnitData == null)
        {
            Debug.Log($"{this.name} UnitData is null");
        }

        Anim = GetComponentInChildren<Animator>();
        if (Anim == null)
        {
            Anim = this.GameObject().AddComponent<Animator>();
            Anim.runtimeAnimatorController = UnitData.UnitAnimator;
        };

        Anims = GetComponentsInChildren<Animator>();

        RB = GetComponent<Rigidbody2D>();
        if (RB == null) RB = this.GameObject().AddComponent<Rigidbody2D>();

        RB.gravityScale = UnitData.UnitGravity;

        CC2D = GetComponent<CapsuleCollider2D>();
        if (CC2D == null) CC2D = this.GameObject().AddComponent<CapsuleCollider2D>();
        CC2D.sharedMaterial = UnitData.UnitCC2DMaterial;

        SR = GetComponent<SpriteRenderer>();
        //if (SR == null) SR = this.GameObject().AddComponent<SpriteRenderer>();

        if (MapSR != null)
        {
            MapSR.gameObject.layer = LayerMask.NameToLayer("Map");
            MapSR.sortingLayerName = "Map";
            MapSR.drawMode = SpriteDrawMode.Sliced;
            MapSR.transform.localScale = Vector3.one;
            MapSR.transform.localRotation = Quaternion.Euler(Vector3.zero);
            MapSR.gameObject.transform.localPosition = Vector3.zero;
            MapSR.transform.localPosition = new Vector3(CC2D.offset.x, CC2D.offset.y, 0);
            MapSR.size = new Vector2(CC2D.size.x, CC2D.size.y);
            MapSR.gameObject.SetActive(true);
        }

        Inventory = GetComponent<Inventory>();
        //if (Inventory == null) Inventory = this.GameObject().AddComponent<Inventory>();

    }

    protected virtual void Start()
    {
        if (GameManager.Inst?.StageManager != null)
        {
            RespawnPoint = GameManager.Inst.StageManager.SpawnPoint.transform;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckLife(this);

        if (Core != null)
            Core.LogicUpdate();
        else
            Debug.Log("Core is null");

        if (Core.CoreUnitStats.invincibleTime >= 0.0f && Core.CoreDamageReceiver.isHit)
        {
            Core.CoreUnitStats.invincibleTime -= Time.deltaTime;

            if (Core.CoreUnitStats.invincibleTime <= 0.0f)
            {
                Core.CoreDamageReceiver.isHit = false;
                Debug.Log(name + " isHit false");
                Core.CoreUnitStats.invincibleTime = 0f;
            }
        }

        if (Core.CoreUnitStats.TouchinvincibleTime >= 0.0f && Core.CoreDamageReceiver.isTouch)
        {
            Core.CoreUnitStats.TouchinvincibleTime -= Time.deltaTime;

            if (Core.CoreUnitStats.TouchinvincibleTime <= 0.0f)
            {
                Core.CoreDamageReceiver.isTouch = false;
                Core.CoreUnitStats.TouchinvincibleTime = 0f;
            }
        }

        FSM.CurrentState.LogicUpdate();
    }


    protected virtual void FixedUpdate()
    {
        FSM.CurrentState.PhysicsUpdate();
    }
    public void SetAnimParam(string str, bool boolean)
    {
        Anim?.SetBool(str, boolean);
    }
    public bool GetAnimBoolParam(string str)
    {
        return Anim.GetBool(str);
    }
    public void SetAnimParam(string str, float _float)
    {
        Anim?.SetFloat(str, _float);
    }
    public float GetAnimFloatParam(string str)
    {
        return Anim.GetFloat(str);
    }
    public void SetAnimParam(string str, int _int)
    {
        Anim?.SetInteger(str, _int);
    }
    public int GetAnimIntParam(string str)
    {
        return Anim.GetInteger(str);
    }
    public virtual void SetTarget(Unit unit)
    {
        //if (unit == null)
        //    return;

        TargetUnit = unit;
    }

    public virtual Unit GetTarget()
    {
        if (TargetUnit == null)
            return null;
        return TargetUnit;
    }

    private void CheckLife(Unit unit)
    {
        //unit.IsAlive = unit.gameObject.activeSelf;
    }


    public void AnimationFinishTrigger() => FSM.CurrentState.AnimationFinishTrigger();

    public virtual void HitEffect()
    {
        foreach (var flash in DamageFlash)
        {
            flash.CallFlashWhite();
        }
    }

    public virtual void DieEffect() { }

    public virtual void Jump() { }
    public void SetActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public IEnumerator DisableCollision()
    {
        Physics2D.IgnoreLayerCollision(this.gameObject.layer, LayerMask.NameToLayer("Platform"), true);
        yield return new WaitForSeconds(0.2f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Platform"), false);
    }
    #endregion Unity Callback Func
}
