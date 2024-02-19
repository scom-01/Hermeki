using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerInputHandler : MonoBehaviour
{
    public PlayerInput playerInput
    {
        get => GetComponent<PlayerInput>();
    }
    PlayerInputActions playerInputActions;
    private Camera cam;
    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    [HideInInspector]
    public bool JumpInput
                    , DashInput
                    , PrimarySkillInput
                    , SecondarySkillInput
                    , PrimaryInput
                    , SecondaryInput
                    , ESCInput
                    , InteractionInput = false;
    [HideInInspector]
    public bool PrimaryInputStop
                    , JumpInputStop
                    , DashInputStop
                    , PrimarySkillInputStop
                    , SecondarySkillInputStop
                    , InteractionInputStop = true;
    [HideInInspector]
    public bool[] ActionInputs;
    [HideInInspector]
    public bool[] ActionInputDelayCheck;

    [SerializeField]
    private float inputHoldTime = 0.2f;

    [HideInInspector]
    public float jumpInputStartTime,
                    dashInputStartTime,
                    PrimarySkillInputStartTime,
                    SecondarySkillInputStartTime,
                    escInputStartTime,
                    interactionInputStartTime = -1;

    [HideInInspector]
    public float interactionMaxHoldDuration = -1;
    public float interactionTapDuration = -1;

    [HideInInspector]
    public bool interactionTap,
                interactionHold = false;

    private float[] ActionInputsStartTime;
    private float[] ActionInputsStopTime;

    /// <summary>
    /// Invoke() 후에 기본 EscInput의 함수호출 x
    /// </summary>
    public Command OnESCInput_Action = new Command();
    /// <summary>
    /// Invoke() 후에 기본 ESCInput 함수호출 o
    /// </summary>
    public Command OnESCInput_MultiAction = new Command();
    private void Awake()
    {
        Init();

        //Debug.Log("This InputHandler ActionMap Name : " + playerInput.currentActionMap.name);
    }

    private void Init()
    {
        if (playerInputActions == null)
            playerInputActions = new PlayerInputActions();
        int count = Enum.GetValues(typeof(CombatInputs)).Length;

        JumpInput = false;
        DashInput = false;
        PrimarySkillInput = false;
        SecondarySkillInput = false;
        PrimaryInput = false;
        ESCInput = false;
        InteractionInput = false;

        PrimaryInputStop = true;
        JumpInputStop = true;
        DashInputStop = true;
        PrimarySkillInputStop = true;
        SecondarySkillInputStop = true;
        InteractionInputStop = true;

        ActionInputs = new bool[count];
        ActionInputDelayCheck = new bool[count];
        for (int i = 0; i < ActionInputDelayCheck.Length; i++)
        {
            ActionInputDelayCheck[i] = true;
        }
        ActionInputsStartTime = new float[count];
        ActionInputsStopTime = new float[count];

        jumpInputStartTime = -1;
        dashInputStartTime = -1;
        PrimarySkillInputStartTime = -1;
        SecondarySkillInputStartTime = -1;
        escInputStartTime = -1;
        interactionInputStartTime = -1;
        if (cam == null)
            cam = Camera.main;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    /// <summary>
    /// Change ActionMap
    /// </summary>
    /// <param name="str">ActionMap Name</param>
    public void SwitchActionMap(string Name)
    {
        playerInput.SwitchCurrentActionMap(Name);
    }

    private void FixedUpdate()
    {
        bool jumpInput = JumpInput;
        bool dashInput = DashInput;
        bool skill1Input = PrimarySkillInput;
        bool skill2Input = SecondarySkillInput;
        //bool interacInput = InteractionInput;
        bool[] attackInputs = ActionInputs;

        CheckHoldTime(ref jumpInput, ref jumpInputStartTime);
        CheckHoldTime(ref dashInput, ref dashInputStartTime);
        CheckHoldTime(ref skill1Input, ref PrimarySkillInputStartTime);
        CheckHoldTime(ref skill2Input, ref SecondarySkillInputStartTime);
        //CheckHoldTime(ref attackInputs, ref ActionInputsStartTime);
        //CheckHoldTime(ref interacInput, ref interactionInputStartTime);

        //JumpInput = jumpInput;
        DashInput = dashInput;
        PrimarySkillInput = skill1Input;
        SecondarySkillInput = skill2Input;
        //ActionInputs = attackInputs;
        //InteractionInput = interacInput;
    }

    #region GamePlay
    //움직임 Input
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        if (Mathf.Abs(RawMovementInput.x) > 0.1 || Mathf.Abs(RawMovementInput.y) > 0.1)
        {
            NormInputX = Mathf.Clamp(Mathf.RoundToInt(10 * RawMovementInput.x), -1, 1);
            NormInputY = Mathf.Clamp(Mathf.RoundToInt(10 * RawMovementInput.y), -1, 1);
        }

        //NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        //NormInputY = Mathf.RoundToInt(RawMovementInput.y);

        if (context.canceled)
        {
            NormInputX = 0;
            NormInputY = 0;
        }
        Debug.Log($"input = ({NormInputX}, {NormInputY})");
    }


    //점프 Input
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnSkill1Input(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("OnSkill 1 Input");
            PrimarySkillInput = true;
            PrimarySkillInputStartTime = Time.time;
            PrimarySkillInputStop = false;
        }

        if (context.canceled)
        {
            PrimarySkillInputStop = true;
        }
    }

    public void OnSkill2Input(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("OnSkill 2 Input");
            SecondarySkillInput = true;
            SecondarySkillInputStartTime = Time.time;
            SecondarySkillInputStop = false;
        }

        if (context.canceled)
        {
            SecondarySkillInputStop = true;
        }
    }

    public void OnPrimaryInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PrimaryInput = true;
            Debug.Log($"OnPrimaryAction = {PrimaryInput}");

            if (ActionInputDelayCheck[(int)CombatInputs.primary])
            {
                ActionInputs[(int)CombatInputs.primary] = true;
                ActionInputsStartTime[(int)CombatInputs.primary] = Time.time;
            }
        }

        if (context.canceled)
        {
            ActionInputs[(int)CombatInputs.primary] = false;
            ActionInputsStopTime[(int)CombatInputs.primary] = Time.time;
        }
    }

    public void OnSecondaryInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SecondaryInput = true;
            Debug.Log($"OnSecondaryInput = {SecondaryInput}");
            if (ActionInputDelayCheck[(int)CombatInputs.secondary])
            {
                ActionInputs[(int)CombatInputs.secondary] = true;
                ActionInputsStartTime[(int)CombatInputs.secondary] = Time.time;
            }
        }

        if (context.canceled)
        {
            ActionInputs[(int)CombatInputs.secondary] = false;
            ActionInputsStopTime[(int)CombatInputs.secondary] = Time.time;
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var tap = context.interaction as TapInteraction;
            if (tap != null)
            {
                interactionTapDuration = tap.duration;
            }

            var hold = context.interaction as HoldInteraction;
            if (hold != null)
            {
                interactionMaxHoldDuration = hold.duration;
            }
            InteractionInput = true;
            InteractionInputStop = false;
            interactionInputStartTime = Time.time;
        }

        if (context.performed)
        {
            if (interactionTapDuration != -1)
                interactionTap = true;

            if (interactionMaxHoldDuration != -1)
                interactionHold = true;
        }

        if (context.canceled)
        {
            InteractionInputStop = true;
            InteractionInput = false;
            interactionTap = false;
            interactionHold = false;
            interactionMaxHoldDuration = -1f;
            interactionTapDuration = -1f;
        }
    }
    #endregion

    #region UI
    public void OnTapInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (GameManager.Inst.StageManager == null)
                return;

            if (OnESCInput_Action.Excute())
            {
                OnESCInput_Action.Clear();
                return;
            }

            Debug.Log("OnTapInput Start");
            if (playerInput.actions.actionMaps.ToArray().Length > 0)
            {
                if (playerInput.currentActionMap == playerInput.actions.FindActionMap(InputEnum.UI.ToString()))
                {
                    ChangeCurrentActionMap(InputEnum.GamePlay, true);
                }
                else if (playerInput.currentActionMap == playerInput.actions.FindActionMap(InputEnum.GamePlay.ToString()))
                {
                    ChangeCurrentActionMap(InputEnum.UI, true);
                }
            }
        }
        if (context.canceled)
        {
            Debug.Log("OnTapInput Cancled");
        }
    }
    public void OnESCInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (OnESCInput_Action.Excute())
            {
                OnESCInput_Action.Clear();
                return;
            }

            if(OnESCInput_MultiAction.Excute())
            {
                OnESCInput_MultiAction.Clear();
            }

            ESCInput = true;
            escInputStartTime = Time.time;
            Debug.Log("OnESCInput Start");

            if (GameManager.Inst?.StageManager != null)
            {
                if (!GameManager.Inst.StageManager.player.IsAlive)
                {
                    return;
                }
            }
            else
            {
                //var _TitleManager = FindObjectOfType(typeof(TitleManager)) as TitleManager;
                //if (_TitleManager != null)
                //{
                //    if (_TitleManager.UnlockItem_Canvas.Canvas.enabled == true)
                //    {
                //        if (_TitleManager.buttons.Count > 0)
                //        {
                //            GameManager.Inst.SetSelectedObject(_TitleManager.buttons[0].gameObject);
                //        }
                //        _TitleManager.UnlockItem_Canvas.Canvas.enabled = false;
                //        return;
                //    }
                //}
            }

            if (playerInput.actions.actionMaps.ToArray().Length > 0)
            {
                //로딩중엔 return
                if (GameManager.Inst.Curr_UIState == UI_State.Loading)
                    return;

                if (playerInput.currentActionMap == playerInput.actions.FindActionMap(InputEnum.UI.ToString()))
                {
                    ChangeCurrentActionMap(InputEnum.GamePlay, true);
                }
                else if (playerInput.currentActionMap == playerInput.actions.FindActionMap(InputEnum.GamePlay.ToString()))
                {
                    ChangeCurrentActionMap(InputEnum.Cfg, true);
                }
                else if (playerInput.currentActionMap == playerInput.actions.FindActionMap(InputEnum.Cfg.ToString()))
                {
                    if (GameManager.Inst.StageManager == null)
                    {
                        ChangeCurrentActionMap(InputEnum.Cfg, true);
                    }
                    else
                    {
                        //foreach (var btn in GameManager.Inst.CfgUI.ConfigPanelUI.cfgBtns)
                        //{
                        //    btn.OnClickActiveUI(false);
                        //}
                        ChangeCurrentActionMap(InputEnum.GamePlay, true);
                    }
                }
            }
        }
        if (context.canceled)
        {
            Debug.Log("OnESCInput Cancled");
            ESCInput = false;
        }
    }
    #endregion

    #region SendMessage
    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();

        if (Mathf.Abs(input.x) > 0.1 || Mathf.Abs(input.y) > 0.1) 
        {
            NormInputX = Mathf.Clamp(Mathf.RoundToInt(10 * input.x), -1, 1);
            NormInputY = Mathf.Clamp(Mathf.RoundToInt(10 * input.y), -1, 1);
        }
        else
        {
            NormInputX = 0;
            NormInputY = 0;
        }
    }

    private void OnJump()
    {
        JumpInput = true;
    }

    private void OnPrimaryAction()
    {
        PrimaryInput = true;
    }
    private void OnSecondaryAction()
    {
        SecondaryInput = true;
    }
    private void OnInteractive()
    {
        InteractionInput = true;
    }
    #endregion

    /// <summary>
    /// Pause가 true이면 현재 isPause상태를 변경하고자 함
    /// </summary>
    /// <param name="inputEnum"></param>
    /// <param name="Pause"></param>
    public void ChangeCurrentActionMap(InputEnum inputEnum, bool Pause, bool Init = false)
    {
        //현재와 동일한 ActionMap으로 변경하려하면 ActionMap변경을 원치않으므로 Pause기능만 하도록
        if (playerInput.currentActionMap == playerInput.actions.FindActionMap(inputEnum.ToString()))
        {
            if (inputEnum == InputEnum.GamePlay)
            {
                return;
            }

            if (Pause)
            {
                GameManager.Inst.CheckPause(inputEnum, Init);
            }
        }
        else
        {
            playerInput.SwitchCurrentActionMap(inputEnum.ToString());

            if (Pause)
            {
                GameManager.Inst.CheckPause(inputEnum, Init);
            }
        }
    }

    public void UseInput(ref bool input) => input = false;

    //홀드 시간

    #region CheckHoldTime
    private void CheckHoldTime(ref bool input, ref float inputStartTime)
    {
        if (Time.time >= inputStartTime + inputHoldTime)
        {
            input = false;
        }
    }
    private void CheckHoldTime(ref int input, ref float inputStartTime)
    {
        if (Time.time >= inputStartTime + inputHoldTime)
        {
            input = 0;
        }
    }
    private void CheckHoldTime(ref float input, ref float inputStartTime)
    {
        if (Time.time >= inputStartTime + inputHoldTime)
        {
            input = 0.0f;
        }
    }
    private void CheckHoldTime(ref bool[] input, ref float[] inputStartTime)
    {
        if (input.Length == 0)
            return;

        for (int i = 0; i < input.Length; i++)
        {
            if (Time.time >= inputStartTime[i] + inputHoldTime)
            {
                {
                    input[i] = false;
                }
            }
        }
    }
    #endregion
    public void Delay(float seconds, ref bool input)
    {
        for (int i = 0; i < ActionInputDelayCheck.Length; i++)
        {
            Debug.Log("Seconds = false");
            ActionInputDelayCheck[i] = false;
            ActionInputs[i] = false;
        }
        StartCoroutine(DelayWait(seconds));

    }
    IEnumerator DelayWait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < ActionInputDelayCheck.Length; i++)
        {
            ActionInputDelayCheck[i] = true;
            ActionInputs[i] = false;
        }
    }

}

public enum CombatInputs
{
    primary,
    secondary,
}