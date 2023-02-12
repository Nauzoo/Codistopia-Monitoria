using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaEvents : EventMannager
{
    [SerializeField] private NPC_Movement Marisa;
    public override void triggerEvent(string eventName)
    {
        switch (eventName)
        {
            case "MarisaT":
                EventsData.happenedEvents.Add("marisaT");
                break;

            case "bookCheck":
                Marisa.animator.SetInteger("animIndex", 0);
                Marisa.canMove = false;
                break;

            case "realeaseBook":
                Marisa.canMove = true;
                break;
        }
    }
}
