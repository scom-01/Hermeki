using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCoreComponent : MonoBehaviour
{
    protected EnemyCore core;

    protected virtual void Awake()
    {
        core = transform.parent.GetComponent<EnemyCore>();

        if (core == null)
        {
            Debug.LogError("There is no Core on the parent");
        }

    }
}
