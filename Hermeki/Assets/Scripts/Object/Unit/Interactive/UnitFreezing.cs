using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFreezing : UnitInteractive
{
    /// <summary>
    /// 효과를 줄 색상
    /// </summary>
    [SerializeField] private Color _Color = Color.white;

    [SerializeField] private Material _Material;
    private Material[] old_materials;

    private float old_speed = 1f;
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    bool isFreezing = false;
    Coroutine _coroutine;
    public override void Interactive(Unit unit)
    {
        if (unit == null)
            return;
        ElapsedTime = 0f;
        Freeze(unit);
    }

    public override void UnInteractive(Unit unit)
    {
        Melt(unit);
    }

    public override void LogicUpdate(Unit unit)
    {
        if (ElapsedTime < DurationTime)
        {
            ElapsedTime += Time.deltaTime;
            float currentAmount = Mathf.Lerp(1f, 0f, (ElapsedTime / DurationTime));
            SetAmout(currentAmount);
        }
    }

    private void Freeze(Unit unit)
    {
        isDuration = true;
        unit.RB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        old_speed = unit.Anim.speed;
        unit.Anim.speed = 0;
        unitSetMaterial(unit, _Material);
        SetColor();

        //_coroutine = StartCoroutine(FreezeCoroutine(unit));
    }

    private void Melt(Unit unit)
    {
        SetMtrl(unit);
        unit.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        unit.Anim.speed = old_speed;
        isDuration = false;
    }
    //IEnumerator FreezeCoroutine(Unit unit)
    //{
    //    unit.RB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    //    float currSpeed = unit.Anim.speed;
    //    unit.Anim.speed = 0;
    //    unitSetMaterial(unit, _Material);
    //    SetColor();
    //    float currentAmount = 0f;
    //    float elapsedTime = 0f;
    //    while (elapsedTime < DurationTime)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        currentAmount = Mathf.Lerp(1f, 0f, (elapsedTime / DurationTime));
    //        SetAmout(currentAmount);
    //        yield return null;
    //    }
    //}

    private void unitSetMaterial(Unit unit, Material mtrl)
    {
        _spriteRenderers = unit.GetComponentsInChildren<SpriteRenderer>();

        _materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = mtrl;
        }

        old_materials = new Material[_spriteRenderers.Length];

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            old_materials[i] = _spriteRenderers[i].material;
            _spriteRenderers[i].material = _materials[i];
        }
    }

    void SetMtrl(Unit unit)
    {
        _spriteRenderers = unit.GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _spriteRenderers[i].material = old_materials[i];
        }
    }

    void SetColor()
    {
        if (_materials.Length == 0)
            return;

        foreach (var mtrl in _materials)
        {
            mtrl.SetColor("_Color", _Color);
        }
    }

    void SetAmout(float amount)
    {
        if (_materials.Length == 0)
            return;

        foreach (var mtrl in _materials)
        {
            mtrl.SetFloat("_Amount", amount);
        }
    }
}
