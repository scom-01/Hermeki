using System.Drawing;
using System.Xml.Linq;
using SCOM;
using UnityEngine;


public class TouchObject : MonoBehaviour, ITouch
{
    protected Transform effectContainer;
    public AudioPrefab SFX;
    public GameObject EffectObject;
    private void Awake()
    {
        effectContainer = GameObject.FindGameObjectWithTag("EffectContainer").transform;
        //this.gameObject.layer = LayerMask.NameToLayer("Area");
    }
    public virtual void Touch(GameObject obj)
    {
        Debug.Log($"Touch = {obj.name}");
    }
    public virtual void UnTouch(GameObject obj)
    {
        Debug.Log($"UnTouch = {obj.name}");
    }
    public virtual void OnTriggerStay2D(Collider2D collision)
    {        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
    }
}
