using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    public static FixedJoystick Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There are more than one intance of the Joystick class!");
        }
    }

    public void FlipActiveState(bool state)
    {
        this.gameObject.SetActive(state);
    }
}