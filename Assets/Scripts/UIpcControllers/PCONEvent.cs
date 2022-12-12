using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCONEvent : MonoBehaviour
{
    [Header("PC UI ELEMENTS")]
    [SerializeField] private Animator monitor;
    [SerializeField] private Animator fader;
    [SerializeField] private GameObject controler;
    [SerializeField] private GameObject interButton;

    
    private void Start()
    {
        
    }

    public void Open()
    {
        fader.SetTrigger("OpenScreen");
        monitor.SetTrigger("OpenScreen");
        controler.SetActive(false);
        interButton.SetActive(false);
    }
    public void Close()
    {
        fader.SetTrigger("CloseScreen");
        monitor.SetTrigger("CloseScreen");
        controler.SetActive(true);
        interButton.SetActive(true);
        
    }
    public void ChangeEvent(int state)
    {
        if (state == 0)
        {
            fader.SetTrigger("OpenScreen");
            monitor.SetTrigger("OpenScreen");
            controler.SetActive(false);
            interButton.SetActive(false);

        }

        if (state == 1)
        {
            fader.SetTrigger("CloseScreen");
            monitor.SetTrigger("CloseScreen");
            controler.SetActive(true);
            interButton.SetActive(true);

        }
    }
}

