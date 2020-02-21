using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FJ
{
    public static class SaveSystem
    {
        public static void SaveHighScore(GameStats gameStats)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath ;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "/highscore.file";
            FileStream stream = new FileStream(path, FileMode.Create);
            PlayerData data = new PlayerData(gameStats);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        public static PlayerData LoadScore()
        {
            string path = Application.persistentDataPath + "/highscore.file";
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
                Debug.LogError("File not found");
                return null;
            }
        }
    }
}