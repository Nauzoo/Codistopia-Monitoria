using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentEvent : MonoBehaviour
{
    [SerializeField] private string specialEvent;
    private DialogTrigger myDialog;
    [SerializeField] private TextAsset myNewDialog;

    private void Start()
    {
        myDialog = GetComponent<DialogTrigger>();
    }

    void Update()
    {
        if (EventsData.happenedEvents.Contains(specialEvent))
        {
            myDialog.dialogFile = myNewDialog;
        }
    }
}
