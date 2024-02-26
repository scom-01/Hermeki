using SCOM;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponStyle
{
    Sword = 0,
    Staff = 1,
}
public class WeaponItem : MonoBehaviour
{
    private Unit unit;
    protected AnimationEventHandler eventHandler;
    private SpriteRenderer sr => GetComponent<SpriteRenderer>();
    private PolygonCollider2D pc2D => GetComponent<PolygonCollider2D>();

    public WeaponStyle weaponStyle;
    public bool isLeft;
    public bool isAction;
    public WeaponItemDataSO dataSO;
    public int CurrentDurability;
    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        if (unit != null)
        {
            this.tag = unit.transform.tag;
        }
        eventHandler = unit.GetComponentInChildren<AnimationEventHandler>();
    }
    private void Start()
    {
        SetWeaponStyle(weaponStyle);
        if (dataSO != null) 
            SetWeapon(new WeaponItemData(dataSO, dataSO.MaxDurability));
    }

    public void SetWeapon(WeaponItemData _data)
    {
        dataSO = _data.dataSO;
        CurrentDurability = _data.CurrentDurability;
        CalculateWeaponSprite();
    }

    [ContextMenu("SetCollider2D")]
    public void SetCollider2D()
    {
        if (sr == null)
            return;

        if (pc2D == null)
            this.AddComponent<PolygonCollider2D>().isTrigger = true;
        PolygonCollider2D polygon = pc2D;

        int shapeCount = sr.sprite.GetPhysicsShapeCount();
        polygon.pathCount = shapeCount;
        var points = new List<Vector2>(64);
        for (int i = 0; i < shapeCount; i++)
        {
            sr.sprite.GetPhysicsShape(i, points);
            polygon.SetPath(i, points);
        }
        polygon.isTrigger = true;
        if (dataSO != null)
        {
            polygon.sharedMaterial = dataSO.PM2D;
        }
    }

    public void SetWeaponData(WeaponItemData _data)
    {
        if (_data == null)
        {
            dataSO = null;
            SetWeaponSprite(null);
            return;
        }
        dataSO = _data.dataSO;
        CurrentDurability = _data.CurrentDurability;
    }

    public void SetWeaponStyle(WeaponStyle style)
    {
        weaponStyle = style;

        if (isLeft)
        {
            unit.SetAnimParam("leftWeapon", (int)style);
        }
        else
        {
            unit.SetAnimParam("rightWeapon", (int)style);
        }
    }

    private void DecreaseDurability()
    {
        CurrentDurability--;
        if (CurrentDurability > 0)
        {
            CalculateWeaponSprite();
            return;
        }
        else
        {
            SetWeaponData(null);
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
            Destroy(pc2D);
        }
    }
    private void CalculateWeaponSprite()
    {
        if (CurrentDurability <= 0 || sr?.sprite == null || dataSO == null)
            return;

        int idx = 0;
        for (int i = 0; i < dataSO.WeaponSprite.Length; i++)
        {
            if (CurrentDurability <= dataSO.WeaponSprite[i].durability)
            {
                idx = i;
            }
            else
            {
                break;
            }
        }
        if (sr.sprite != dataSO.WeaponSprite[idx].sprite)
        {
            SetWeaponSprite(dataSO.WeaponSprite[idx].sprite);
            SetCollider2D();
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

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!isAction)
            return;

        //Ground
        if (coll.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (isLeft)
            {
                eventHandler.FinishLeftAction();
            }
            else
            {
                eventHandler.FinishRightAction();
            }
            DecreaseDurability();
            if (dataSO != null)
                Effect(this.transform, dataSO.Grounded_effectData, dataSO.Grounded_audioData);
            Debug.Log($"Weapon Collider Hit Ground");
            return;
        }
        //Unit
        if (coll.gameObject.layer == LayerMask.NameToLayer("Damageable"))
        {
            if (coll.CompareTag(this.gameObject.tag))
                return;

            //Hit시 효과
            if (coll.TryGetComponent(out IDamageable victim))
            {
                if (victim.Damage(unit, 1, 1) == 0)
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
                DecreaseDurability();
                if (dataSO != null)
                    Effect(coll.transform, dataSO.Unit_effectData, dataSO.Unit_audioData);
            }
            Debug.Log($"Weapon Collider Hit Unit");
        }
    }

    private void Action()
    {
        isAction = true;
        switch (weaponStyle)
        {
            case WeaponStyle.Sword:
                break;
            case WeaponStyle.Staff:
                break;
            default:
                break;
        }
        if (isLeft)
            Debug.Log($"StartAction Left = {(unit as Player).InputHandler.PrimaryInput}");
        else
            Debug.Log($"StartAction Right = {(unit as Player).InputHandler.PrimaryInput}");

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
}
