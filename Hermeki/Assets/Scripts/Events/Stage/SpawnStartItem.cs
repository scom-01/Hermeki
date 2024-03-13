using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnStartItem : ActionEvent
{
    public List<GameObject> equipItems = new List<GameObject>();
    public Transform SpawnPos;
    public float interval = 2f;
    protected override void Action()
    {
        if (equipItems?.Count == 0 && SpawnPos == null)
            return;

        for (int i = 0; i < equipItems.Count; i++)
        {
            if (equipItems[i] == null)
                continue;
            GameObject obj = Instantiate(equipItems[i]);
            obj.transform.position = SpawnPos.position + Vector3.right * interval * i;
        }
    }
    public void SetEquipItem(List<GameObject> objects)
    {
        if (objects == null)
            return;
        equipItems = objects.ToList();
    }
}
