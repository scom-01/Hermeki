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

    public override bool Interactive()
    {
        if (isDone)
            return false;

        Debug.LogWarning("OpenGate");
        GameManager.Inst.InputHandler.ChangeCurrentActionMap(InputEnum.Cfg, false);
        GameManager.Inst?.ClearScene();
        isDone = true;
        return true;
    }
}
