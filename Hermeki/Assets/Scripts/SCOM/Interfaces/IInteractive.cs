using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractive
{
    public virtual bool Interactive() { return true; }
    public virtual bool UnInteractive() { return true; }
}
