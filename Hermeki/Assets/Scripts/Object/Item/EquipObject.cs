using UnityEngine;

public abstract class EquipObject : MonoBehaviour, IInteractive
{
    public abstract void Interactive(Unit unit);
    public abstract void UnInteractive(Unit unit);
    public abstract void SetSpriteRenderer();
    public EquipItemData Data;
    public Item_Type Type;

    /// <summary>
    /// true : 장착가능, 아이템 드랍 후 바로 줍는 상황 방지
    /// </summary>
    private bool isEquipable = false;

    public EquipObject(EquipItemData data)
    {
        Data = data;
    }

    protected virtual void Start()
    {
        SetSpriteRenderer();
    }

    public virtual bool SetData(EquipItemData _data)
    {
        if (_data == null)
            return false;
        Data = _data;
        Invoke("Setquipabletrue", 0.25f);
        SetSpriteRenderer();
        return true;
    }
    private void Setquipabletrue() => isEquipable = true;

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
