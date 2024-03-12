using System.Collections.Generic;
using UnityEngine;

public class EditorEnemySpawnManager : MonoBehaviour
{
    public List<GameObject> EnemyList = new List<GameObject>();

    public float interval;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            Spawn(transform.position + Vector3.right * (i * interval), EnemyList[i]);
        }        
    }

    private void Spawn(Vector3 pos, GameObject enemy)
    {
            GameObject _enemy = Instantiate(enemy);
            _enemy.transform.position = pos;
    }
}
