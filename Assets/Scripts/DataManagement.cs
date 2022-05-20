using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManagement : MonoBehaviour
{
    public static DataManagement dataManagement;
    public int highScore;
    void Awake()
    {
        if (dataManagement == null)
        {
            DontDestroyOnLoad(gameObject);
            dataManagement = this;
        }
        else if(dataManagement != this)
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        BinaryFormatter BinForm = new BinaryFormatter(); // Creates a bin formatter
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat"); // Creates file
        gameData data = new gameData(); // creates container for data
        data.highScore = highScore;
        BinForm.Serialize(file, data); // serializes
        file.Close();
    }

    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter BinForm = new BinaryFormatter(); // Creates a bin formatter
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open); 
            gameData data = (gameData)BinForm.Deserialize(file);
            file.Close();
            highScore = data.highScore;
        }
    }
}

[Serializable]
class gameData
{
    public int highScore;

}