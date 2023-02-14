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
    }

    public static Vector2 LoadPlayer()
    {        
        string path = Application.persistentDataPath + "/playerData.txt";
        if (File.ReadAllText(path) != "nullity")
        {
            
            string pPos = File.ReadAllText(path);

            bool _ = float.TryParse(pPos.Split(":")[1].Split(";")[0], out float Xpos);
            bool __ = float.TryParse(pPos.Split(":")[1].Split(";")[1], out float Ypos);

            Vector2 lPos = new Vector2(Xpos, Ypos);

            return lPos;
            
        }
        else
        {
            return Vector2.zero;
        }
       
    }

    public static void SaveEvents()
    {
        string path = Application.persistentDataPath + "/eventData.txt";

        string formated = "";
        if (EventsData.happenedEvents.Count > 0)
        {
            foreach (string hEvent in EventsData.happenedEvents)
            {
                formated += hEvent + ";";
            }
        }

        File.WriteAllText(path, formated);

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
    }

    public static void SaveLastScene(int ID)
    {
        string path = Application.persistentDataPath + "/sceneData.txt";
        File.WriteAllText(path, $"LAST_SAVED_SCENE:{ID}");
    }

    public static void LoadLastScene()
    {
        string path = Application.persistentDataPath + "/sceneData.txt";
        if (File.ReadAllText(path) != "nullity")
        {

            string lScen = File.ReadAllText(path);

            lScen = lScen.Split(":")[1];

            SavedSceneData.savedScene = int.Parse(lScen);
        }
        else
        {
            SavedSceneData.savedScene = 2;
        }
    }
    public static void SaveVick(VickFollower vick)
    {
        string path = Application.persistentDataPath + "/vickyData.txt";
        Vector2 pPos = vick.transform.position;

        File.WriteAllText(path, $"POSITION:{pPos[0]};{pPos[1]}");
    }
    public static Vector2 LoadVick()
    {
        string path = Application.persistentDataPath + "/vickyData.txt";
        if (File.ReadAllText(path) != "nullity")
        {

            string pPos = File.ReadAllText(path);

            bool _ = float.TryParse(pPos.Split(":")[1].Split(";")[0], out float Xpos);
            bool __ = float.TryParse(pPos.Split(":")[1].Split(";")[1], out float Ypos);

            Vector2 lPos = new Vector2(Xpos, Ypos);

            return lPos;

        }
        else
        {
            return Vector2.zero;
        }

    }
    
}
