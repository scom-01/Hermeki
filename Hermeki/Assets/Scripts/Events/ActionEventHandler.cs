using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventHandler : MonoBehaviour
{
    private event Action StartEventAction;
    private event Action EndEventAction;
    public void StartAction() => StartEventAction?.Invoke();
    public void EndAction() => EndEventAction?.Invoke();
    public void AddStartAction(Action action)
    {
        StartEventAction -= action;
        StartEventAction += action;
    }
    public void AddEndAction(Action action)
    {
        EndEventAction -= action;
        EndEventAction += action;
    }
    public void RemoveStartAction(Action action)
    {
        StartEventAction -= action;
    }
    public void RemoveEndAction(Action action)
    {
        EndEventAction -= action;
    }
}
