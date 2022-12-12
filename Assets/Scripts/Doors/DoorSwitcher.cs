using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitcher : MonoBehaviour
{
    [SerializeField] private string MyEvent;
    public bool MyState;

    void Start()
    {
        if (EventsData.happenedEvents.Contains(MyEvent))
        {
            this.gameObject.SetActive(MyState);
        }
        else
        {
            this.gameObject.SetActive(!MyState);
        }
        Debug.Log("activated");
    }
    
    public void SwitchState()
    { 
        this.gameObject.SetActive(MyState);
    }
}
