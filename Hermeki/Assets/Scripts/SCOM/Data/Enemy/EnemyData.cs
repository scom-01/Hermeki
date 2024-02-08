using Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class EnemyData : UnitData
{
    [Header("Type")]
    public ENEMY_Form enemy_form = ENEMY_Form.Grounded;
    [Tooltip("Enemy의 소속")]
    [TagField]
    public string enemy_tag;
    public ENEMY_Level enemy_level = ENEMY_Level.NormalEnemy;

    /// <summary>
    /// 유닛 공격 가능 거리
    /// </summary>
    [Header("EnemyAttack")]
    public float UnitAttackDistance;

    /// <summary>
    /// 패턴 딜레이
    /// </summary>
    [Header("StateData")]
    [Min(0.25f)]
    public float minIdleTime;
    [Min(0.5f)]
    public float maxIdleTime;
}
