using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Events;
using System;
using System.Globalization;

public static class Save 
{
    public static string SavePath { get; set; } = Application.dataPath;
    public static UnityEvent<long> ScoreChanged = new UnityEvent<long>();

    public static void SaveData()
    {
        LoadData loadData = new LoadData();
        GameData gameData = GameData.GetInstance();
        loadData.Score = gameData.Score;
        loadData.BonusScoreToRoll = gameData.BonusScoreToRoll;
        loadData.SecondsAfkFarm = gameData.SecondsAfkFarm;
        loadData.LastVisit = DateTime.UtcNow.ToString("u", CultureInfo.InvariantCulture);
        Debug.LogWarning(loadData.LastVisit);
        loadData.CountUpgrade = gameData.CountUpgrade;
        loadData.DiceAfkFarm = gameData.DiceAfkFarm;
        loadData.Dices = gameData.Dices;
        File.WriteAllText(
            SavePath + "/Save.json",
            JsonConvert.SerializeObject(loadData, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore //Отмена залупливания обращений 
            })
            );
    }
    public static void Load()
    {
        LoadData loadData;
        try
        {
            loadData = JsonConvert.DeserializeObject<LoadData>(File.ReadAllText(SavePath + "/Save.json"));
        }
        catch (FileNotFoundException)
        {

            return;
        }
        
        if (loadData != null)
        {
            GameData gameData = GameData.GetInstance();
            gameData.Score = loadData.Score;
            gameData.BonusScoreToRoll = loadData.BonusScoreToRoll;
            gameData.SecondsAfkFarm = loadData.SecondsAfkFarm;
            gameData.LastVisit = loadData.LastVisit;
            Debug.LogError(loadData.LastVisit);
            Debug.LogError(gameData.LastVisit);
            gameData.CountUpgrade = loadData.CountUpgrade;
            gameData.DiceAfkFarm = loadData.DiceAfkFarm;
            gameData.Dices = loadData.Dices;
            ScoreChanged?.Invoke(gameData.Score);
        }
           
    }
}

public class LoadData {
    public long Score { get; set; }
    public ushort BonusScoreToRoll { get; set; }
    public List<DiceEnum> Dices { get; set; }
    public DiceEnum DiceAfkFarm { get; set; }
    public int SecondsAfkFarm { get; set; }
    public string LastVisit { get; set; }
    public uint CountUpgrade { get; set; }
}
