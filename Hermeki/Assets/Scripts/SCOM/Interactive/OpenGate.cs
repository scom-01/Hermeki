using UnityEngine;

public class OpenGate : InteractiveObject
{
    private bool isDone = false;

    private void Awake()
    {
        isDone = false;
    }
    protected override void Start()
    {
        base.Start();
    }

    public override void Interactive()
    {
        if (isDone)
            return;

        Debug.LogWarning("OpenGate");
        GameManager.Inst.InputHandler.ChangeCurrentActionMap(InputEnum.Cfg, false);
        GameManager.Inst?.ClearScene();
        isDone = true;
    }
}
