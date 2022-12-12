using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionMannager : MonoBehaviour
{
    private static InteractionMannager instance;
    private bool CurrentState; // Keeps if the interaction button is being pressed (true) or not (false)

    // Creating a singleton class (A class that is static and needs to be unique in a scene)
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There are more than one instace of Interaction mannager on this scene!");
        }
        instance = this;
    }
    public static InteractionMannager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        CurrentState = false; // Setting the interation state to false by default
    }
    public void OpenInteraction() {
        CurrentState = true; // On interaction event call (pressing the button)
        return;
    }
    public void CloseInteraction() {
        CurrentState = false; // On interaction event call (realing the button)
        return;
    }

    public bool Interaction() {
        return CurrentState; // returns the button current state
        
    }
}
