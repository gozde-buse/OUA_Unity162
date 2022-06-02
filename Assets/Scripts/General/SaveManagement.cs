using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManagement
{
    public static void Save()
    {
        string path = Application.persistentDataPath + "/player.data";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void Load()
    {
        string path = Application.persistentDataPath + "/player.data";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            data.LoadData();
            stream.Close();
        }
    }

    public static void CreateData()
    {
        string path = Application.persistentDataPath + "/player.data";

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(1);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static bool ControlData()
    {
        string path = Application.persistentDataPath + "/player.data";

        if (File.Exists(path))
            return true;

        return false;
    }

    //Silinecek
    public static void DeleteData()
    {
        string path = Application.persistentDataPath + "/player.data";

        if (File.Exists(path))
            File.Delete(path);
    }
}

[Serializable]
public class PlayerData
{
    public int level;
    public string levelStars;

    public PlayerData()
    {
        level = PlayerStats.level;
        levelStars = "";

        for (int i = 0; i < PlayerStats.levelStars.Length; i++)
            levelStars += PlayerStats.levelStars[i].ToString();
    }

    public PlayerData(int level)
    {
        level = 1;
        levelStars = "";

        for (int i = 0; i < PlayerStats.levelStars.Length; i++)
            levelStars += 0.ToString();
    }

    public void LoadData()
    {
        PlayerStats.SetLevel(level);

        for (int i = 0; i < PlayerStats.levelStars.Length; i++)
            PlayerStats.SetLevelStar(i, levelStars[i]);
    }
}