using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct Panel_Data
{ 
    public GameObject panel;
    public bool isOnOff;
}

public class PanelUI : MonoBehaviour
{
    /// <summary>
    /// OnOff하고자 하는 Panel List
    /// </summary>
    public List<Panel_Data> PanelList;
    [HideInInspector]
    public List<bool> isOnOffCheckList = new List<bool>();
    protected virtual void Awake()
    {
        isOnOffCheckList.Clear();
        for (int i = 0; i < PanelList.Count; i++)
        {
            isOnOffCheckList.Add(PanelList[i].isOnOff);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        for (int i = 0; i < isOnOffCheckList.Count; i++)
        {
            if (isOnOffCheckList[i] == PanelList[i].isOnOff)
                continue;

            if (PanelList[i].isOnOff)
                PanelList[i].panel?.SetActive(true);
            else
                PanelList[i].panel?.SetActive(false);

            isOnOffCheckList[i] = PanelList[i].isOnOff;
        }
    }
}