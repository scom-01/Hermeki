using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RespawnArea : TouchObject
{
    public Transform RespawnPoint;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        var unit = collision.GetComponent<Unit>();
        if (unit == null)
        {
            return;
        }

        if (this.RespawnPoint == null)
            return;

        unit.RespawnPoint = this.RespawnPoint;
    }
}
