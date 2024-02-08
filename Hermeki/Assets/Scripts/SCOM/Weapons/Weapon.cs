using SCOM.CoreSystem;
using SCOM.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SCOM.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [field:SerializeField] public List<CommandEnum> CommandList = new List<CommandEnum>();
        public WeaponGenerator weaponGenerator 
        { 
            get
            {
                return this.gameObject.GetComponent<WeaponGenerator>();
            }
        }

        //Component

        public WeaponData weaponData;

        public Core WeaponCore { get; private set; }
        public GameObject BaseGameObject { get; private set; }
        public GameObject WeaponSpriteGameObject { get; private set; }
        public AnimationEventHandler EventHandler { get; private set; }

        //현재 커맨드상태
        [HideInInspector] public CommandEnum Command;
        [HideInInspector] public AnimatorOverrideController oc;
        protected UnitState state;
        
        private Timer actionCounterResetTimer;

        //Action
        public event Action OnEnter;
        public event Action OnExit;

        //Value
        [Tooltip("공격 횟수")]  public int CurrentActionCounter
        {
            get => currentActionCounter;
            private set => currentActionCounter = value >= weaponData.weaponDataSO.NumberOfActions ? 0 : value;
        }
        [HideInInspector]   public bool InAir;
        [HideInInspector]   protected string weaponAnimBoolStr;
        
        protected Animator baseAnimator;
        
        [SerializeField]    private float actionCounterResetCooldown;

        private int currentActionCounter;

        /// <summary>
        /// Primary Skill StartTime
        /// </summary>
        public float PrimarySkillStartTime = 0f;
        /// <summary>
        /// Secondary Skill StartTime
        /// </summary>
        public float SecondarySkillStartTime = 0f;

        protected virtual void Awake()
        {
            BaseGameObject = transform.GetComponentInChildren<Animator>().gameObject;
            WeaponSpriteGameObject = transform.Find("Weapon").gameObject;

            baseAnimator = BaseGameObject.GetComponent<Animator>();

            EventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();
            
            actionCounterResetTimer = new Timer(actionCounterResetCooldown);

            this.gameObject.tag = this.GetComponentInParent<Unit>().gameObject.tag;
            this.gameObject.layer = this.GetComponentInParent<Unit>().gameObject.layer;
        }
        private void Start()
        {
            WeaponCore.CoreUnitStats.OnChangeStats += ChangeWeaponSpeed;
        }

        private void Update()
        {
            if (WeaponCore == null)
                return;

            
            actionCounterResetTimer.Tick();
        }
        private void ChangeWeaponSpeed()
        {
            BaseGameObject.GetComponent<Animator>().speed = (WeaponCore.CoreUnitStats.CalculStatsData.DefaultAttSpeed/ 100) * ((100 + WeaponCore.CoreUnitStats.CalculStatsData.AttackSpeedPer) / 100);
        }

        private void FixedUpdate()
        {
            if (WeaponCore == null)
                return;
            if(GlobalValue.ContainParam(baseAnimator, "inAir"))
            {
                SetBoolName("inAir", InAir);
            }
        }

        private void OnEnable()
        {
            //TODO: event Action 사용 시 Finish처리가 animation 끝부분에 있기에 Cancle처리가 안됨
            EventHandler.OnFinish += ExitWeapon;            
            actionCounterResetTimer.OnTimerDone += ResetActionCounter;
        }

        private void OnDisable()
        {
            EventHandler.OnFinish -= ExitWeapon;
            actionCounterResetTimer.OnTimerDone -= ResetActionCounter;
        }

        /// <summary>
        /// PlayerWeaponState Enter 에서 호출되는 함수
        /// </summary>
        public virtual void EnterWeapon()
        {
            print($"{transform.name} EnterWeapon");

            actionCounterResetTimer.StopTimer();
            if (oc != null)
            {
                baseAnimator.runtimeAnimatorController = oc;
            }
            if (GlobalValue.ContainParam(baseAnimator, "action"))
            {
                baseAnimator.GetComponent<AnimationEventHandler>().isAction = true;
                SetBoolName("action", true);
            }
            if (GlobalValue.ContainParam(baseAnimator, "actionCounter"))
                SetIntName("actionCounter", CurrentActionCounter);
            OnEnter?.Invoke();
            ChangeWeaponSpeed();
        }

        /// <summary>
        /// PlayerWeaponState Exit 에서 호출되는 함수
        /// </summary>
        public virtual void ExitWeapon()
        {
            Debug.Log("Exit Weapon");
            WeaponCore.Unit.RB.gravityScale = WeaponCore.Unit.UnitData.UnitGravity;
            SetBoolName("action", false);
            CurrentActionCounter++;
            actionCounterResetTimer.StartTimer();
            WeaponCore.Unit.AnimationFinishTrigger();

            OnExit?.Invoke();
        }

        public void SetCore(Core core)
        {
            if(core == null)
            {
                Debug.Log($"{transform.name} is core Null");
                return;
            }
            WeaponCore = core;
        }
        public void SetData(WeaponDataSO data)
        {
            this.weaponData.weaponDataSO = data;
        }
        public void SetCommandData(WeaponCommandDataSO data)
        {
            this.weaponData.weaponCommandDataSO = data;

            weaponGenerator.weaponData.weaponCommandDataSO = data;
            if(WeaponCore.Unit.GetType() == typeof(Player))
            {
                //Weapon Skill Sprite Panel Setting
                //GameManager.Inst?.MainUI?.MainPanel?.SkillPanelSystem?.PrimarySkillPanel?.Init(weaponGenerator.weaponData.weaponCommandDataSO.PrimarySkillData.SkillSprite);
                //GameManager.Inst?.MainUI?.MainPanel?.SkillPanelSystem?.SecondarySkillPanel?.Init(weaponGenerator.weaponData.weaponCommandDataSO.SecondarySkillData.SkillSprite);
            }
            weaponGenerator.GenerateWeapon(data.DefaultWeaponDataSO);
        }

        public void InitSkillStartTime()
        {
            //Weapon Command 변경 시 Primary, Secondary Start Time 초기화
            PrimarySkillStartTime = 0f;
            SecondarySkillStartTime = 0f;
        }

        public void ChangeActionCounter(int value)
        {
            CurrentActionCounter = value;
            ResetCommandList();
        }

        public void ResetActionCounter()
        {
            ResetCommandList();
            CurrentActionCounter = 0;            
        }
        /// <summary>
        /// 커맨드 List 초기화
        /// </summary>
        public void ResetCommandList() => CommandList.Clear();
        #region Set Func

        /// <summary>
        /// Animator Parameter Set Func
        /// </summary>
        /// <param name="name"> Param Name</param>
        /// <param name="setbool">T, F</param>
        public virtual void SetBoolName(string name, bool setbool)
        {
            weaponAnimBoolStr = name;
            baseAnimator.SetBool(name, setbool);
        }
        public virtual void SetIntName(string name, int setint)
        {
            weaponAnimBoolStr = name;
            baseAnimator.SetInteger(name, setint);
        }
        public virtual void SetFloatName(string name, int setfloat)
        {
            weaponAnimBoolStr = name;
            baseAnimator.SetFloat(name, setfloat);
        }
        #endregion


        /// <summary>
        /// 초기 State 설정
        /// </summary>
        /// <param name="state"></param>
        public void InitializeWeapon(UnitState state, Core core)
        {
            this.state = state;
            this.WeaponCore = core;
        }
#if UNITY_EDITOR
        public void OnGUI()
        {
            if(CommandList.Count > 0)
            {
                for(int i =0;i < CommandList.Count;i++)
                {
                    GUI.Label(new Rect(5, 5 * (i + 1), Screen.width, 20), CommandList[i].ToString());
                }
            }
        }
#endif
    }
}