using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnStartItem : StartActionEvent
{
    public List<GameObject> EquipItems = new List<GameObject>();
    public Transform SpawnPos;
    public float interval = 2f;
    protected override void Action()
    {
        if (EquipItems?.Count == 0 && SpawnPos == null)
            return;

        for (int i = 0; i < EquipItems.Count; i++)
        {
            if (EquipItems[i] == null)
                continue;
            GameObject obj = Instantiate(EquipItems[i]);
            obj.transform.position = SpawnPos.position + Vector3.right * interval * i;
        }
    }
    public bool SetEquipItem(List<GameObject> objects)
    {
        if (objects == null)
            return false;
        EquipItems = objects.ToList();
        return true;
    }
}
