using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipObject : MonoBehaviour, IInteractive
{
    public abstract void Interactive(Unit unit);
    public abstract void UnInteractive(Unit unit);
    public abstract void SetSpriteRenderer();
    public EquipItemData Data;

    public EquipObject(EquipItemData data)
    {
        Data = data;
    }

    protected virtual void Start()
    {
        SetSpriteRenderer();
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(this.tag)|| !collision.CompareTag("Player"))
            return;

        //Ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;

        //Unit
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            if (!collision.GetComponent<Unit>().IsAlive)
                return;

            Interactive(collision.GetComponent<Unit>());
            Debug.Log($"TriggerEnter Unit name = {collision.gameObject.name}");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(this.tag))
            return;

        //Ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;

        //Unit
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            UnInteractive(collision.GetComponent<Unit>());
            Debug.Log($"TriggerExit Unit name = {collision.gameObject.name}");
        }
    }
}
