using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public abstract class EventMannager : MonoBehaviour
{
    public abstract void triggerEvent(string eventName);


    public void SaveGame()
    {
        SavingController.SavePlayer(Player_movement.GetInstance());
        SavingController.SaveVick(VickFollower.GetInstance());
        SavingController.SaveEvents();
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        SavingController.SaveLastScene(sceneId);
        SoundMannager.Instance.PlaySound(SoundMannager.Instance.Sfx_Save);
    }
    public void savingSceneConf()
    {
        LastScene.lastPassedScene = SceneManager.GetActiveScene().buildIndex;
        LastScene.lastPlayerPos = Player_movement.GetInstance().transform.position;
    }
}
