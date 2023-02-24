using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingController: MonoBehaviour
{
    public static SavingController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public static void SaveGame(GameSave gameData)
    {        
        BinaryFormatter byformatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(GetPath(), FileMode.Create);
        byformatter.Serialize(fileStream, gameData);
        fileStream.Close();

    }
    public static GameSave LoadGame()
    {
        if (!File.Exists(GetPath()))
        {
            GameSave emptyData = new GameSave();
            SaveGame(emptyData);
            return emptyData;
        }
        BinaryFormatter byformatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(GetPath(), FileMode.Open);
        GameSave data = byformatter.Deserialize(fileStream) as GameSave;
        fileStream.Close();

        return data;
    }

    public static string GetPath()
    {
        return Application.persistentDataPath + "/saveData.nut";
    }
    
}
