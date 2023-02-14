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
        SavingController.SavePlayer(Player_movement.GetInstance());
        SavingController.SaveVick(VickFollower.GetInstance());
        SavingController.SaveEvents();
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        SavingController.SaveLastScene(sceneId);
        SoundMannager.Instance.PlaySound(SoundMannager.Instance.Sfx_Save);
    }
    public static void DeleteSave()
    {
        Debug.Log("Deleting save data...");

        string path1 = Application.persistentDataPath + "/playerData.txt";
        string path2 = Application.persistentDataPath + "/eventData.txt";
        string path3 = Application.persistentDataPath + "/sceneData.txt";
        File.WriteAllText(path1, "nullity");
        File.WriteAllText(path2, "");
        File.WriteAllText(path3, "nullity");
    }
    public void savingSceneConf()
    {
        LastScene.lastPassedScene = SceneManager.GetActiveScene().buildIndex;
        LastScene.lastPlayerPos = Player_movement.GetInstance().transform.position;
    }
}
