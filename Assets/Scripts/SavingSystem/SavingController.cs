using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingController
{
    public static void SavePlayer (Player_movement player)
    {
        string path = Application.persistentDataPath + "/playerData.txt";
        Vector2 pPos = player.transform.position;

        File.WriteAllText(path, $"POSITION:{pPos[0]};{pPos[1]}");

        /*BinaryFormatter binaryFormatter = new BinaryFormatter();
        
        FileStream fileStream = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(player);

        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();*/
    }

    public static Vector2 LoadPlayer()
    {
        string path = Application.persistentDataPath + "/playerData.txt";
        string pPos = File.ReadAllText(path);

        bool _ = float.TryParse(pPos.Split(":")[1].Split(";")[0], out float Xpos);
        bool __ = float.TryParse(pPos.Split(":")[1].Split(";")[1], out float Ypos);

        Vector2 lPos = new Vector2(Xpos, Ypos);

        return lPos;

        /*
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Open);

        PlayerData playerLoad = binaryFormatter.Deserialize(fileStream) as PlayerData;
        fileStream.Close();

        return playerLoad.position;*/
    }

    public static void SaveEvents()
    {
        string path = Application.persistentDataPath + "/eventData.txt";

        string formated = "";
        foreach (string hEvent in EventsData.happenedEvents)
        {
            formated += hEvent + ";";
        }

        File.WriteAllText(path, formated);
        
        /*BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/eventSave.nut";
        FileStream fileStream = new FileStream(path, FileMode.Create);
                

        binaryFormatter.Serialize(fileStream, EventsData.happenedEvents);
        fileStream.Close();*/
    }
    public static List<string> LoadEvents()
    {
        string path = Application.persistentDataPath + "/eventData.txt";

        if (File.Exists(path))
        {
            List<string> dEvents = new List<string>();

            string[] data = File.ReadAllText(path).Split(";");

            foreach (string hEvent in data)
            {
                dEvents.Add(hEvent);
            }

            return dEvents;
        }
        else
        {
            return null;
        }

        /*if (File.Exists(path))
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
        }*/
    }

    public static void SaveLastScene(int ID)
    {
        string path = Application.persistentDataPath + "/sceneData.txt";
        File.WriteAllText(path, $"LAST_SAVED_SCENE:{ID}");

        /*BinaryFormatter binaryFormatter = new BinaryFormatter();

        FileStream fileStream = new FileStream(path, FileMode.Create);
        SavedSceneData lastScene = new SavedSceneData();
        lastScene.holder= ID;

        binaryFormatter.Serialize(fileStream, lastScene);
        fileStream.Close();*/
    }

    public static void LoadLastScene()
    {
        string path = Application.persistentDataPath + "/sceneData.txt";
        string lScen = File.ReadAllText(path);

        lScen = lScen.Split(":")[1];

        SavedSceneData.savedScene = int.Parse(lScen);

        /*BinaryFormatter binaryFormatter = new BinaryFormatter();
        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);

            SavedSceneData lastScene = binaryFormatter.Deserialize(fileStream) as SavedSceneData;
            fileStream.Close();
            SavedSceneData.savedScene = lastScene.holder;
            
        }
        else
        {
            Debug.LogError("Tried to access an unexisting file");
            
        }*/

    }
    public static void SaveVick(VickFollower vick)
    {
        /*BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/vickSave.nut";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        VickyData vickData = new VickyData(vick);

        binaryFormatter.Serialize(fileStream, vickData);
        fileStream.Close();*/
    }
    public static Vector2 LoadVick()
    {
        /*string path = Application.persistentDataPath + "/vickSave.nut";
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path, FileMode.Open);

        VickyData vickLoad = binaryFormatter.Deserialize(fileStream) as VickyData;
        fileStream.Close();

        return vickLoad.position;*/
        return Vector2.zero;
        
    }
    
}
