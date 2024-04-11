using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSetActive : TouchObject
{
    public List<GameObject> objects = new List<GameObject>();

    private void Awake()
    {
        foreach (var _obj in objects) _obj.SetActive(false);
    }
    public override void Touch(GameObject obj)
    {
        base.Touch(obj);
        foreach (var _obj in objects) _obj.SetActive(true);
    }

    public override void UnTouch(GameObject obj)
    {
        base.UnTouch(obj);
        foreach (var _obj in objects) _obj.SetActive(false);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (!collision.CompareTag("Player") || collision.GetComponent<Unit>() == null)
            return;

        Touch(collision.gameObject);
    }

    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        
        if (!collision.CompareTag("Player") || collision.GetComponent<Unit>() == null)
            return;
        
        UnTouch(collision.gameObject);
    }
}
