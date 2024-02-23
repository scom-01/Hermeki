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
    public bool isTrigger { set { if (pc2D != null) pc2D.isTrigger = value; } }

    public WeaponStyle weaponStyle;
    public bool isLeft;
    public bool isAction;
    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        if(unit != null)
        {
            this.tag = unit.transform.tag;
        }
        eventHandler = unit.GetComponentInChildren<AnimationEventHandler>();
    }
    private void Start()
    {
        SetWeaponStyle(weaponStyle);
    }

    [ContextMenu("SetCollider2D")]
    public void SetCollider2D()
    {
        if (sr == null && pc2D == null)
            return;
        
        DestroyImmediate(this.GetComponent<PolygonCollider2D>());
        if (sr.sprite == null)
        {
            return;
        }

        this.AddComponent<PolygonCollider2D>().isTrigger = true;
    }

    public void SetWeaponStyle(WeaponStyle style)
    {
        weaponStyle = style;

        if(isLeft)
        {
            unit.SetAnimParam("leftWeapon", (int)style);
        }
        else
        {
            unit.SetAnimParam("rightWeapon", (int)style);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAction)
            return;

        //Ground
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (isLeft)
            {
                eventHandler.FinishLeftAction();
            }
            else
            {
                eventHandler.FinishRightAction();
            }
            Debug.Log($"Weapon Collider Hit Ground");
            return;
        }
        //Unit
        if (collision.gameObject.layer == LayerMask.NameToLayer("Damageable"))
        {
            if(collision.tag == this.gameObject.tag)
                return;
            
            //Hit시 효과
            if (collision.TryGetComponent(out IDamageable victim))
            {
                if (victim.Damage(unit, 1, 1) == 0)
                {
                    return;
                }
            }

            if (isLeft)
            {
                eventHandler.FinishLeftAction();
            }
            else
            {
                eventHandler.FinishRightAction();
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
        }
        else
        {
            (unit as Player).InputHandler.UseInput(ref (unit as Player).InputHandler.SecondaryInput);
        }
    }

    private void OnEnable()
    {
        if(isLeft)
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
        if(isLeft)
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
