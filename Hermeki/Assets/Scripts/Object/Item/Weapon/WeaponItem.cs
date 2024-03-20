using SCOM;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class WeaponItem : EquipItem
{
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();
    private PolygonCollider2D pc2D => GetComponent<PolygonCollider2D>();

    public bool isLeft;
    public bool isAction;
    public override EquipItemData GetData(out EquipItemData data)
    {
        data = unit?.ItemManager?.AllItemList[10 + (isLeft ? 0 : 1)];
        return data;
    }
    protected override void Start()
    {
        base.Start();
        if (Data?.dataSO != null)
            SetItemData(new EquipItemData(Data.dataSO, Data.dataSO.MaxDurability));
        SetCollider2D();
    }

    #region Variable
    public override void DecreaseDurability()
    {
        base.DecreaseDurability();

        if (Data.CurrentDurability > 0)
        {
            CalculateWeaponSprite();
            return;
        }
        else
        {
            Data.CurrentDurability = 0;
            SetItemData(null);
            Debug.Log($"Destroy Weapon {this.name}");
        }
    }

    private void SetWeaponSprite(UnityEngine.Sprite sprite)
    {
        if (sr == null)
        {
            return;
        }
        sr.sprite = sprite;
        if (sprite == null && pc2D != null)
        {
            pc2D.enabled = false;
            //Destroy(pc2D);
        }
    }
    private void CalculateWeaponSprite()
    {
        if (Data.CurrentDurability <= 0 || Data?.dataSO == null)
            return;
                
        //무기의 Sprite는 1개 이므로 sprites[0]으로 고정
        if (Data.CalculateSprite()[0] != null && sr.sprite != Data.CalculateSprite()[0])
        {
            SetWeaponSprite(Data.CalculateSprite()[0]);
            SetCollider2D();
        }
    }

    #endregion

    #region Collider
    [ContextMenu("SetCollider2D")]
    public void SetCollider2D()
    {
        if (pc2D == null)
            this.AddComponent<PolygonCollider2D>().isTrigger = true;

        if (sr?.sprite == null)
        {
            pc2D.enabled = false;
            return;
        }
        PolygonCollider2D polygon = pc2D;

        polygon.enabled = true;
        int shapeCount = sr.sprite.GetPhysicsShapeCount();
        polygon.pathCount = shapeCount;
        var points = new List<Vector2>(64);
        for (int i = 0; i < shapeCount; i++)
        {
            sr.sprite.GetPhysicsShape(i, points);
            polygon.SetPath(i, points);
        }
        polygon.isTrigger = true;
        if (Data?.dataSO != null)
        {
            polygon.sharedMaterial = (Data.dataSO as WeaponItemDataSO).PM2D;
        }
    }
    #endregion

    public override bool SetItemData(EquipItemData _data)
    {
        if (!base.SetItemData(_data))
        {
            DestroyItem();
            Data.dataSO = null;
            SetWeaponSprite(null);
            SetWeaponStyle(WeaponStyle.Sword);
            return false;
        }

        //아이템 장착 배열에 위치
        unit.ItemManager.AllItemList[10 + (isLeft ? 0 : 1)].SetEquipItemData(_data);

        CalculateWeaponSprite();
        SetWeaponStyle((Data.dataSO as WeaponItemDataSO).Style);
        return false;
    }

    public override bool DestroyItem()
    {
        if (base.DestroyItem())
        {
            return false;
        }

        return true;
    }
    public void SetWeaponStyle(WeaponStyle style)
    {
        if (isLeft)
        {
            unit.SetAnimParam("leftWeapon", (int)style);
        }
        else
        {
            unit.SetAnimParam("rightWeapon", (int)style);
        }
    }


    private void Effect(Vector2 _pos, EffectPrefab effectData, AudioData audioData)
    {
        effectData.SpawnObject(_pos);
        SoundManager.Inst?.Play(audioData, Sound.Effect);
        Debug.Log($"Effect transform Pos = {_pos}");
    }
    private void Effect(Transform _transform, EffectPrefab effectData, AudioData audioData)
    {
        effectData.SpawnObject(_transform.position);
        SoundManager.Inst?.Play(audioData, Sound.Effect);
        Debug.Log($"Effect transform = {_transform}");
    }

    #region Action
    private void Action()
    {
        isAction = true;
        
        if (isLeft)
            Debug.Log($"StartAction Left = {(unit as Player).InputHandler.PrimaryInput}");
        else
            Debug.Log($"StartAction Right = {(unit as Player).InputHandler.PrimaryInput}");
        
        unit.ItemManager?.ExeItemEvent(ItemEvent, ItemEvent_Type.OnAction);
        
        if (Data?.dataSO != null)
        {
            switch ((Data.dataSO as WeaponItemDataSO).Style)
            {
                case WeaponStyle.Sword:
                    break;
                case WeaponStyle.Staff:
                    DecreaseDurability();
                    break;
                default:
                    break;
            }
        }
    }

    private void FinishAction()
    {
        if ((unit as Player)?.InputHandler == null)
        {
            return;
        }

        isAction = false;
        if (isLeft)
        {
            (unit as Player).InputHandler.UseInput(ref (unit as Player).InputHandler.PrimaryInput);
            Debug.Log($"FinishAction Left = {(unit as Player).InputHandler.PrimaryInput}");
        }
        else
        {
            (unit as Player).InputHandler.UseInput(ref (unit as Player).InputHandler.SecondaryInput);
            Debug.Log($"FinishAction Right = {(unit as Player).InputHandler.SecondaryInput}");
        }
    }

    #endregion
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isAction || Data?.dataSO == null)
            return;

        //Ground
        if (coll.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            ///지형 충돌 무시
            if (!(Data.dataSO as WeaponItemDataSO).isPhysicsCheck_Ground)
                return;
            
            if (isLeft)
            {
                eventHandler.FinishLeftAction();
            }
            else
            {
                eventHandler.FinishRightAction();
            }
            unit.ItemManager?.ExeItemEvent(ItemEvent, ItemEvent_Type.OnHitGround);
            DecreaseDurability();
            if (Data?.dataSO != null)
            {
                Effect(this.transform, (Data.dataSO as WeaponItemDataSO).Grounded_effectData, (Data.dataSO as WeaponItemDataSO).Grounded_audioData);
            }
            Debug.Log($"Weapon Collider Hit Ground");
            return;
        }
        //Unit
        if (coll.gameObject.layer == LayerMask.NameToLayer("Damageable"))
        {
            if (coll.CompareTag(this.gameObject.tag))
                return;

            ///지형 충돌 무시
            if (!(Data.dataSO as WeaponItemDataSO).isPhysicsCheck_Unit)
                return;

            //Hit시 효과
            if (coll.TryGetComponent(out IDamageable victim))
            {
                if (victim.Damage(unit, 1) == 0)
                {
                    return;
                }
                if (isLeft)
                {
                    eventHandler.FinishLeftAction();
                }
                else
                {
                    eventHandler.FinishRightAction();
                }
                unit.ItemManager.ExeItemEvent(ItemEvent, ItemEvent_Type.OnHitEnemy, coll.GetComponentInParent<Unit>());
                DecreaseDurability();
                if (Data?.dataSO != null)
                    Effect(coll.transform, (Data.dataSO as WeaponItemDataSO).Unit_effectData, (Data.dataSO as WeaponItemDataSO).Unit_audioData);
            }
            Debug.Log($"Weapon Collider Hit Unit");
        }
    }


    #region OnEvent
    private void OnEnable()
    {
        if (isLeft)
        {
            eventHandler.OnLeftAction -= Action;
            eventHandler.OnLeftAction += Action;
            eventHandler.OnLeftActionFinish -= FinishAction;
            eventHandler.OnLeftActionFinish += FinishAction;
        }
        else
        {
            eventHandler.OnRightAction -= Action;
            eventHandler.OnRightAction += Action;
            eventHandler.OnRightActionFinish -= FinishAction;
            eventHandler.OnRightActionFinish += FinishAction;
        }
    }

    private void OnDisable()
    {
        if (isLeft)
        {
            eventHandler.OnLeftAction -= Action;
            eventHandler.OnLeftActionFinish -= FinishAction;
        }
        else
        {
            eventHandler.OnRightAction -= Action;
            eventHandler.OnRightActionFinish -= FinishAction;
        }
    }
    #endregion
}
