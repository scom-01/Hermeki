using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackHealthBarEffect : MonoBehaviour
{
    /// <summary>
    /// 보간 시간을 조절합니다.
    /// </summary>
    private float lerpduration = 0.25f;
    private float startDelay = 0.25f; // 시작 딜레이를 설정합니다.

    public float Health_value
    {
        get => m_Health_value;
        set
        {
            if (Mathf.Approximately(m_Health_value, value)) // 이전 값과 변경된 값이 거의 같으면 업데이트를 수행하지 않습니다.
                return;

            m_Health_value = value;
            OnChange?.Invoke();
        }
    }
    private float m_Health_value;
    public event Action OnChange;
    private Slider Slider
    {
        get
        {
            if(slider ==null)
            {
                slider = this.GetComponent<Slider>();
            }
            return slider;
        }
    }
    private Slider slider;

    private void OnEnable() => OnChange += UpdateHealthBar;
    private void OnDisable() => OnChange -= UpdateHealthBar;
    private void UpdateHealthBar()
    {
        StartCoroutine(StartUpdateHealthEffect());
    }

    IEnumerator StartUpdateHealthEffect()
    {
        yield return new WaitForSeconds(startDelay);

        StartCoroutine(Update_HealthEffect());
    }

    public IEnumerator Update_HealthEffect()
    {
        if (Slider == null)
            yield break;

        float targetValue = Health_value;
        float startTime = Time.time;

        while (Mathf.Abs(Slider.value - targetValue) > 0.001f)
        {
            float elapsedTime = Time.time - startTime;
            float lerpValue = Mathf.Lerp(Slider.value, targetValue, elapsedTime / lerpduration);
            Slider.value = lerpValue;

            yield return null;
        }
    }
}
