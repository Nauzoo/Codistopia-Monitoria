using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveKiller : MonoBehaviour
{
    public static void DELETE_SAVE()
    {
        string path1 = Application.persistentDataPath + "/playerData.txt";
        string path2 = Application.persistentDataPath + "/eventData.txt";
        string path3 = Application.persistentDataPath + "/sceneData.txt";
        if (File.Exists(path1))
        {
            File.WriteAllText(path1, "");
        }
        if (File.Exists(path2))
        {
            File.WriteAllText(path2, "");
        }
        if (File.Exists(path3))
        {
            File.WriteAllText(path3, "");
        }

    }
}
