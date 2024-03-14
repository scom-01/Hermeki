public class EndActionEvent : ActionEvent
{
    protected override void Start()
    {
        base.Start();
        actionEventHandler.AddEndAction(Action);
    }
}
