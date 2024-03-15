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

        actionEventHandler.AddStartAction(Action);
        actionEventHandler.AddEndAction(UnAction);
    }
    protected virtual void Action() { }
    protected virtual void UnAction() { }
}
