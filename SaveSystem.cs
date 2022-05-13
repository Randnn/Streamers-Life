using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveSystem
{ 
    public static void SaveGame(GameManager player, int a)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedata" + a.ToString() + ".fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer (int a)
    {
        string path = Application.persistentDataPath + "/savedata" + a.ToString() + ".fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }

    public static void NewGame(int a)
    {

        string path = Application.persistentDataPath + "/savedata" + a.ToString() + ".fun";
        if (File.Exists(path))
        {
            Debug.LogError("Save Game Already Exists");
        }
        else
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
        }   
    }
}
