using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEventHandler : MonoBehaviour
{
    public event Action EventAction;
    public void Action() => EventAction?.Invoke();
}
