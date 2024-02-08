using UnityEngine;

public interface IDamageable
{
    public float Damage(Unit attacker, float amount, int repeat);
    public float TypeCalDamage(Unit attacker, float AttackerDmg, int RepeatAmount = 1);
    public float FixedDamage(int amount, bool isTrueHit = false, int RepeatAmount = 1);
    public float FixedDamage(Unit attacker, int amount, bool isTrueHit = false, int RepeatAmount = 1);
    public void HitEffect(GameObject EffectPrefab, float Range, int FancingDirection, Vector3 size);
    public void HitEffectRandRot(GameObject EffectPrefab, float Range, Vector3 size, bool isFollow = false);
}
