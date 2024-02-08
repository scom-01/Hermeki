using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1. DamageFlash 효과를 주기위한 오브젝트에 컴포넌트로 붙이기
/// 2. 해당 오브젝트의 SpriteRender를 FlashWhite로 설정
/// 3. 특정 컬러 및 지속시간 설정
/// </summary>
public class DamageFlash : MonoBehaviour
{
    /// <summary>
    /// 효과를 줄 색상
    /// </summary>
    [SerializeField] private Color _flashColor = Color.white;
    /// <summary>
    /// 효과 지속시간
    /// </summary>
    [SerializeField] private float _flashTime = 0.25f;

    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    private Coroutine _Coroutine;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Init();
    }
    void Init()
    {
        _materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
        }
    }

    public void CallFlashWhite()
    {
        _Coroutine = StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        SetFlashColor();
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            SetFlashAmout(currentFlashAmount);
            yield return null;
        }
    }

    void SetFlashColor()
    {
        foreach (var mtrl in _materials)
        {
            mtrl.SetColor("_FlashColor", _flashColor);
        }
    }

    void SetFlashAmout(float amount)
    {
        foreach (var mtrl in _materials)
        {
            mtrl.SetFloat("_FlashAmount", amount);
        }
    }
}