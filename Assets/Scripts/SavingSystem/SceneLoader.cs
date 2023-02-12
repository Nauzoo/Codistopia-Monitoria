using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{    

    private static SceneLoader instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public SceneLoader GetInsatnce()
    {
        return instance;
    }
    private void Start()
    {
        Vector2 playerPos = LastScene.lastPlayerPos;
        Vector2 vickPos = LastScene.lastVickPos;

        Player_movement player = Player_movement.GetInstance();
        VickFollower vick = VickFollower.GetInstance();
        
        player.transform.position = playerPos;
        vick.gameObject.transform.position = vickPos;

        if (SavingController.LoadEvents() != null)
        {
            foreach (string events in SavingController.LoadEvents())
            {
                EventsData.happenedEvents.Add(events);
            }
        }
    }
    
}
