using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;

public abstract class EventMannager : MonoBehaviour
{
    public abstract void triggerEvent(string eventName);


    public void SaveGame()
    {
        GameSave data = new GameSave();
        data.hasSave = true;
        Vector3 playerPos = Player_movement.GetInstance().transform.position;
        Vector3 vickyPos = VickFollower.GetInstance().transform.position;
        data.playerPosition = new float[] { playerPos[0], playerPos[1] };
        data.vickyPosition = new float[] { vickyPos[0], vickyPos[1] };
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        data.SavedScene = sceneId;
        data.happendEvents = EventsData.happenedEvents.ToArray();
        SavingController.SaveGame(data);
        SoundMannager.Instance.PlaySound(SoundMannager.Instance.Sfx_Save);
    }
    public static void DeleteSave()
    {
        Debug.Log("Deleting save data...");

        GameSave nullData = new GameSave();
        SavingController.SaveGame(nullData);

    }
    public void ChangeToScene(int sceneIndex)
    {
        ChangeScene.Instance.ChangeToScene(sceneIndex);
    }
    public void saveSceneConf()
    {
        LastScene.lastPassedScene = SceneManager.GetActiveScene().buildIndex;
        LastScene.lastPlayerPos = Player_movement.GetInstance().transform.position;
    }
}
