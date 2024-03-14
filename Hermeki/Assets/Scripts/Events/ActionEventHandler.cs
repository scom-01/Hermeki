using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventHandler : MonoBehaviour
{
    private event Action StartEventAction;
    private event Action EndEventAction;
    public void StartAction() => StartEventAction?.Invoke();
    public void AddStartAction(Action action)
    {
        StartEventAction -= action;
        StartEventAction += action;
    }
    public void EndAction() => EndEventAction?.Invoke();
    public void AddEndAction(Action action)
    {
        EndEventAction -= action;
        EndEventAction += action;
    }
}
