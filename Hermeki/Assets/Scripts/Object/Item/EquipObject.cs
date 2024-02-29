using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipObject : MonoBehaviour,IInteractive
{
    public abstract void Interactive(Unit unit);
    public abstract void UnInteractive(Unit unit);

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(this.tag))
            return;

        //Ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;

        //Unit
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
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
