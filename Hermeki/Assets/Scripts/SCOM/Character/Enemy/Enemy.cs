using SCOM.CoreSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Pattern_Data
{
    /// <summary>
    /// 패턴 사용 여부
    /// </summary>
    public bool Used = false;
    /// <summary>
    /// 패턴의 감지 범위 (적과의 점 By 점 길이)
    /// </summary>
    public float Detected_Distance;
    /// <summary>
    /// 현재 패턴의 체력 경계선(이 이하면 해당 패턴 벗어남)
    /// Of이면 패턴의 경계를 나누지 않음
    /// </summary>
    [Min(0f)]
    [Tooltip("Of이면 패턴의 경계를 나누지 않음")]
    public float Boundary;

    public ENEMY_DetectedType DetectedType;
}

public abstract  class Enemy : Unit
{
    #region State Variables
    [HideInInspector]
    public EnemyData enemyData;

    [SerializeField] private float Test_Distance;
    [SerializeField] public List<Pattern_Data> Pattern_Idx;
    #endregion

    #region Unity Callback Func
    protected override void Awake()
    {
        base.Awake();
        enemyData = UnitData as EnemyData;
        gameObject.tag = enemyData.enemy_tag;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Inventory?.Weapon.SetCore(Core);
        Init();
    }

    protected virtual void Init()
    {
        
    }
    public virtual void EnemyPattern() { }
    public override void DieEffect()
    {
        //var item = Inventory.Items;
        //int count = item.Count;

        //var goods = this.GetComponentsInChildren<GoodsItem>();
        //if (goods != null)
        //{
        //    foreach(var good in goods)
        //    {
        //        good.SpawnGoods();
        //    }
        //}

        if (GameManager.Inst != null)
        {
            //GameManager.Inst.StageManager.SPM.UIEnemyCount--;
            //DataManager.Inst.SetEnemyCount(enemyData.enemy_level, 1);
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(Core.CoreCollisionSenses.GroundCenterPos + new Vector3(0, CC2D.offset.y, 0),
            Core.CoreCollisionSenses.GroundCenterPos + new Vector3(0, CC2D.offset.y, 0) + Vector3.right * Core.CoreMovement.FancingDirection * Test_Distance);
    }
}
