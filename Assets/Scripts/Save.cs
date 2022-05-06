using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.Events;

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
        loadData.HoursAfkFarm = gameData.HoursAfkFarm;
        loadData.CountUpgrade = gameData.CountUpgrade;
        loadData.DiceAfkfarm = gameData.DiceAfkfarm;
        loadData.Dices = gameData.Dices;
        File.WriteAllText(
            SavePath + "/Save.json",
            JsonConvert.SerializeObject(GameData.GetInstance(), Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore //Отмена залупливания обращений 
            })
            );
    }
    public static void Load()
    {
        LoadData loadData = JsonConvert.DeserializeObject<LoadData>(File.ReadAllText(SavePath + "/Save.json"));
        if (loadData != null)
        {
            GameData gameData = GameData.GetInstance();
            gameData.Score = loadData.Score;
            gameData.BonusScoreToRoll = loadData.BonusScoreToRoll;
            gameData.HoursAfkFarm = loadData.HoursAfkFarm;
            gameData.CountUpgrade = loadData.CountUpgrade;
            gameData.DiceAfkfarm = loadData.DiceAfkfarm;
            gameData.Dices = loadData.Dices;
            ScoreChanged?.Invoke(gameData.Score);
        }
           
    }
}

public class LoadData {
    public long Score { get; set; }
    public ushort BonusScoreToRoll { get; set; }
    public List<DiceEnum> Dices { get; set; }
    public DiceEnum DiceAfkfarm { get; set; }
    public ushort HoursAfkFarm { get; set; }
    public uint CountUpgrade { get; set; }
}
