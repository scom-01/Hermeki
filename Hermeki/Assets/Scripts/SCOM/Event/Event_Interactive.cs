using UnityEngine;


public class Event_Interactive : MonoBehaviour
{
    [SerializeField] private InteractiveObject interactiveObject;

    public void Start_Action()
    {
        interactiveObject?.Start_Action();
    }
    public void End_Action()
    {
        interactiveObject?.End_Action();
    }
    public void UnInteractive()
    {
        interactiveObject?.UnInteractive();
    }
    public void Interactive()
    {
        interactiveObject?.Interactive();
    }
}
