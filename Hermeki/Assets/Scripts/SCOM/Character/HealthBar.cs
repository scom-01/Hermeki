using SCOM.CoreSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private bool isPlayer;
    private Unit Unit
    {
        get
        {
            if (unit == null)
            {
                if (isPlayer)
                {
                    unit = GameManager.Inst?.StageManager?.player;
                }
                else
                {
                    unit = this.GetComponentInParent<Unit>();
                    
                }
                if (unit != null)
                {
                    m_Canvas.worldCamera = m_Camera;
                    Stats.OnChangeHealth -= UpdateBar;
                    Stats.OnChangeHealth += UpdateBar;
                    UpdateBar();
                }
            }
            return unit;
        }
    }
    private Unit unit;
    private UnitStats Stats
    {
        get
        {
            if (stats == null)
            {
                if (Unit == null)
                {
                    return null;
                }
                stats = Unit.Core.CoreUnitStats;
            }
            return stats;
        }
    }
    private UnitStats stats;
    private Slider m_Slider
    {
        get
        {
            if (m_slider == null)
            {
                m_slider = this.GetComponent<Slider>();
            }
            return m_slider;
        }
    }
    private Slider m_slider;
    [SerializeField] private bool m_IsFollow = true;
    [SerializeField] private bool m_IsShowTxt = true;
    [SerializeField] private TextMeshProUGUI Txt;
    [SerializeField]
    private Camera Cam
    {
        get
        {
            if (m_Camera == null)
            {
                m_Camera = Camera.main; // 필요한 경우에만 Camera 변수를 사용합니다.
                if (GameManager.Inst.StageManager.Cam != null)
                {
                    m_Camera = GameManager.Inst.StageManager.Cam;
                }
            }
            return m_Camera;
        }
    }
    private Camera m_Camera;

    private BackHealthBarEffect HealthBarEffect
    {
        get
        {
            if (m_HealthBarEffect == null)
            {
                m_HealthBarEffect = this.GetComponentInChildren<BackHealthBarEffect>();
            }
            return m_HealthBarEffect;
        }
    }
    private BackHealthBarEffect m_HealthBarEffect;
    [SerializeField] private float lerpduration = 0.5f;
    private Coroutine runningCoroutine;

    private void Start()
    {
        if (m_Slider != null)
            m_Slider.value = 1f;

        UpdateBar();
    }
    private void FixedUpdate()
    {
        if (Unit == null)
            return;

        if (!m_IsFollow)
            return;

        m_Canvas.transform.localRotation = unit.transform.rotation;
    }
    private void OnEnable()
    {
        if (Stats != null)
        {
            Stats.OnChangeHealth -= UpdateBar;
            Stats.OnChangeHealth += UpdateBar;
        }
    }
    private void OnDisable()
    {
        if (Stats != null)
        {
            Stats.OnChangeHealth -= UpdateBar;
        }
    }

    private void UpdateBar()
    {
        if (Stats == null)
            return;

        // 슬라이더의 목표 값을 계산합니다.
        float targetValue = Stats.CurrentHealth / Stats.CalculStatsData.MaxHealth;
        if (m_Slider != null)
            m_Slider.value = targetValue;
        //if (runningCoroutine != null)
        //{
        //    StopCoroutine(runningCoroutine);
        //}
        //runningCoroutine = StartCoroutine(LerpHealthBar(targetValue));
        if (Txt != null)
        {
            if (m_IsShowTxt)
            {
                Txt.gameObject.SetActive(true);
                Txt.text = string.Format($"{(int)Stats.CurrentHealth} / {Stats.CalculStatsData.MaxHealth}");
            }
            else
            {
                Txt.gameObject.SetActive(false);
            }
        }

        if (m_Slider != null)
            CallBackHealthBarEffect();
    }

    //Lerp
    IEnumerator LerpHealthBar(float targetValue)
    {
        if (m_Slider != null)
        {
            float lerpValue = m_Slider.value;
            float startTime = Time.time;

            while (lerpValue > targetValue)
            {
                float elapsedTime = Time.time - startTime;
                lerpValue = Mathf.Lerp(m_Slider.value, targetValue, elapsedTime / lerpduration);
                m_Slider.value = lerpValue;

                yield return null;
            }

            CallBackHealthBarEffect();
        }
    }

    void CallBackHealthBarEffect()
    {
        Debug.Log("Success UpdateHealthBar");
        if (HealthBarEffect != null)
            HealthBarEffect.Health_value = m_Slider.value;
    }

    private Canvas m_Canvas
    {
        get
        {
            if (_canvas == null)
            {
                _canvas = this.GetComponentInParent<Canvas>();
            }
            return _canvas;
        }
    }
    private Canvas _canvas;
}
