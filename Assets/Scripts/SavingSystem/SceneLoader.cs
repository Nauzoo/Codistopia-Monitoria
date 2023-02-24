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
        if (LastScene.lastPlayerPos != Vector2.zero)
        {
            Vector2 playerPos = LastScene.lastPlayerPos;
            Player_movement player = Player_movement.GetInstance();
            player.transform.position = playerPos;

        }
        if (LastScene.lastVickPos != Vector2.zero)
        {
            Vector2 vickPos = LastScene.lastVickPos;
            VickFollower vick = VickFollower.GetInstance();
            vick.gameObject.transform.position = vickPos;
        }

        GameSave data = SavingController.LoadGame();
        if (data.happendEvents != null)
        {
            foreach (string events in data.happendEvents)
            {
                EventsData.happenedEvents.Add(events);
            }
        }
    }
    
}
