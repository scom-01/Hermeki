using UnityEngine;

public class ActionEvent : MonoBehaviour
{
    public ActionEventHandler actionEventHandler;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        actionEventHandler = this.GetComponentInParent<ActionEventHandler>();
        if (actionEventHandler == null)
            return;
    }
    protected virtual void Action() { }
}
