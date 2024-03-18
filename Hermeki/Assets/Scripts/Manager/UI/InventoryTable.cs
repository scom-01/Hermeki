using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class InventoryTable : MonoBehaviour
{
    Canvas canvas;
    List<Canvas> TableList;
    public InventoryState State;
    private void Awake()
    {
        canvas = this.GetComponent<Canvas>();
        TableList = this.GetComponentsInChildren<Canvas>().ToList();
        if (TableList != null)
        {
            TableList.Remove(canvas);
        }
    }

    public void SetState(int idx)
    {
        SetState((InventoryState)idx);
    }
    public void SetState(InventoryState _state)
    {
        if(_state == InventoryState.Close)
        {
            canvas.enabled = false;
        }
        else
        {
            canvas.enabled = true;
        }

        State = _state;

        for (int i = 0; i < TableList.Count; i++)
        {
            if (i == (int)_state)
            {
                TableList[i].enabled = true;                
            }
            else
            {
                TableList[i].enabled = false;
            }
        }
    }
}
