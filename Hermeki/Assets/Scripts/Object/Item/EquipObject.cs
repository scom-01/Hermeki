using UnityEngine;

public abstract class EquipObject : MonoBehaviour, IInteractive
{
    public abstract void Interactive(Unit unit);
    public abstract void UnInteractive(Unit unit);
    public abstract void SetSpriteRenderer();
    public EquipItemData Data;
    public Item_Type Type;

    public bool isEquipable = true;

    public EquipObject(EquipItemData data)
    {
        Data = data;
        isEquipable = true;
    }

    protected virtual void Start()
    {
        SetSpriteRenderer();
    }

    public virtual bool SetData(EquipItemData _data, bool _isEquipable = false)
    {
        if (_data == null)
            return false;
        Data = _data;
        isEquipable = _isEquipable;
        SetSpriteRenderer();
        return true;
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!CheckUnit(collision.gameObject))
            return;
        
        if (!collision.GetComponent<Unit>().IsAlive)
            return;

        if(isEquipable)
        {
            Interactive(collision.GetComponent<Unit>());
            Debug.Log($"TriggerEnter Unit name = {collision.gameObject.name}");        
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!CheckUnit(collision.gameObject))
            return;
        
        if (!collision.GetComponent<Unit>().IsAlive)
            return;

        isEquipable = true;
        UnInteractive(collision.GetComponent<Unit>());
        Debug.Log($"TriggerExit Unit name = {collision.gameObject.name}");
    }

    private bool CheckUnit(GameObject _obj)
    {
        if (_obj.CompareTag(this.tag) || !_obj.CompareTag("Player"))
            return false;

        //Ground
        if (_obj.layer == LayerMask.NameToLayer("Ground"))
            return false;

        //Unit
        if (_obj.layer != LayerMask.NameToLayer("Unit"))
            return false;

        return true;
    }
}
