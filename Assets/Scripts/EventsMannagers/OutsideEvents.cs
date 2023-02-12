using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideEvents : EventMannager
{

    [SerializeField] private DoorSwitcher[] Wdoor;
    public override void triggerEvent(string eventName)
    {
        switch (eventName)
        {
            case "unlockW":
                EventsData.happenedEvents.Add("unlockW");
                Debug.Log(eventName);
                foreach (DoorSwitcher element in Wdoor)
                {
                    element.SwitchState();
                }
                break;
            
            case "SaveGame":
                SaveGame();
                break;
        }
    }
}
