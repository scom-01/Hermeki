using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageEndPoint : InteractiveObject
{
    [TagField]
    public string Tag;
    public override bool Interactive()
    {
        if (!this.CompareTag(Tag))
            return false;
        return true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != Interactive_Layer)
        {
            return;
        }

        if (!Interactive())
        {
            return;
        }
    }
}
