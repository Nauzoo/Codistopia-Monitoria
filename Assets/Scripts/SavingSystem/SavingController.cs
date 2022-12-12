using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingController
{   
    public static void SavePlayer (Player_movement player)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerSave.nut";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(player);

        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerSave.nut";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            PlayerData playerLoad = binaryFormatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();

            return playerLoad;

        } else
        {
            Debug.LogError("Tried to access an unexisting file");
            return null;
        }
    }

    public static void SaveEvents()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/eventSave.nut";
        FileStream fileStream = new FileStream(path, FileMode.Create);
                

        binaryFormatter.Serialize(fileStream, EventsData.happenedEvents);
        fileStream.Close();
    }
    public static List<string> LoadEvents()
    {
        string path = Application.persistentDataPath + "/eventSave.nut";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            List<string> events = binaryFormatter.Deserialize(fileStream) as List<string>;
            fileStream.Close();

            return events;
        }
        else
        {
            Debug.LogError("Tried to access an unexisting file");
            return null;
        }
    }

    public static void SaveLastScene(int ID)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/lasScene.nut";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        SavedSceneData lastScene = new SavedSceneData();
        lastScene.savedScene = ID;

        binaryFormatter.Serialize(fileStream, lastScene);
        fileStream.Close();
    }

    public static SavedSceneData LoadLastScene()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/lasScene.nut";
        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);

            SavedSceneData lastScene = binaryFormatter.Deserialize(fileStream) as SavedSceneData;
            fileStream.Close();

            return lastScene;
        }
        else
        {
            Debug.LogError("Tried to access an unexisting file");
            return null;
        }

    }
    public static void SaveVick(VickFollower vick)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/vickSave.nut";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        VickyData vickData = new VickyData(vick);

        binaryFormatter.Serialize(fileStream, vickData);
        fileStream.Close();
    }
    public static VickyData LoadVick()
    {
        string path = Application.persistentDataPath + "/vickSave.nut";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            VickyData vickLoad = binaryFormatter.Deserialize(fileStream) as VickyData;
            fileStream.Close();

            return vickLoad;

        }
        else
        {
            Debug.LogError("Tried to access an unexisting file");
            return null;
        }
    }
    
}
