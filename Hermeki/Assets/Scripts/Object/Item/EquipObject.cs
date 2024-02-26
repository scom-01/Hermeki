using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipObject : MonoBehaviour,IInteractive
{    
    public void Interactive()
    {
        
    }

    public void UnInteractive()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(this.tag))
            return;

        //Ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            return;

        //Unit
        if (collision.gameObject.layer == LayerMask.NameToLayer("Unit"))
        {
            Interactive();
            Debug.Log($"Touched Unit name = {collision.gameObject.name}");
        }
    }
}
