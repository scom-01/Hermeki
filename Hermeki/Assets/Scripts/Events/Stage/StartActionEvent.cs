public class StartActionEvent : ActionEvent
{
    protected override void Start()
    {
        base.Start();
        actionEventHandler.AddStartAction(Action);
    }
}
