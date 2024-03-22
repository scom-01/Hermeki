using UnityEngine;

public class ActionEvent : MonoBehaviour
{
    public ActionEventHandler actionEventHandler;

    protected virtual void Awake()
    {
        actionEventHandler = this.GetComponentInParent<ActionEventHandler>();
        if (actionEventHandler == null)
            return;

        actionEventHandler.AddStartAction(Action);
        actionEventHandler.AddEndAction(UnAction);
    }
    protected void OnEnable()
    {
        actionEventHandler.AddStartAction(Action);
        actionEventHandler.AddEndAction(UnAction);
    }

    protected void OnDisable()
    {
        actionEventHandler.RemoveStartAction(Action);
        actionEventHandler.RemoveEndAction(UnAction);        
    }
    protected virtual void Action() { }
    protected virtual void UnAction() { }
}
