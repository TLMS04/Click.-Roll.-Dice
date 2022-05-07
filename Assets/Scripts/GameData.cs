using System;
using System.Collections.Generic;
using System.Globalization;

public class GameData
{
    private static GameData _instance;

    private long _score;
    private ushort _bonusScoreToRoll;
    private List<DiceEnum> _dices;
    private DiceEnum _diceAfkfarm;
    private int _secondsAfkFarm;
    private string _lastVisit;
    private uint _countUpgrade;
    private GameData() { }
    private GameData(long score, ushort bonusScoreToRoll, int secondsAfkFarm, uint countUpgrade, DiceEnum diceAfkfarm, List<DiceEnum> dices) 
    {
        _score = score;
        _bonusScoreToRoll = bonusScoreToRoll;
        _secondsAfkFarm = secondsAfkFarm;
        _countUpgrade = countUpgrade;
        _diceAfkfarm = diceAfkfarm;
        _dices = dices;
    }
    public static GameData GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameData();
            _instance.Score = 0;
            _instance.BonusScoreToRoll = 0;
            _instance.CountUpgrade = 1;
            _instance.Dices = new List<DiceEnum>();
            _instance.DicesAdd(DiceEnum.d4);
            _instance.SecondsAfkFarm = 0;
            _instance.DiceAfkFarm = DiceEnum.d4;
        }
            
        return _instance;
    }
    public static GameData GetInstance(long score, ushort bonusScoreToRoll, int secondsAfkFarm, uint countUpgrade, DiceEnum diceAfkfarm, List<DiceEnum> dices)
    {
        if (_instance == null)
            _instance = new GameData(score, bonusScoreToRoll, secondsAfkFarm, countUpgrade, diceAfkfarm,  dices);
        return _instance;
    }

    public long Score { get { return _score; } set { _score = value; } }
    public ushort BonusScoreToRoll { get { return _bonusScoreToRoll; } set { _bonusScoreToRoll = value; } }
    public int SecondsAfkFarm { get { return _secondsAfkFarm; } set { _secondsAfkFarm = value; } }
    public string LastVisit { get { return _lastVisit; } set { _lastVisit = value; } }
    public uint CountUpgrade { get { return _countUpgrade; } set { _countUpgrade = value; } }
    public DiceEnum DiceAfkFarm { get { return _diceAfkfarm; } set { _diceAfkfarm = value; } }
    public List<DiceEnum> Dices { get { return _dices; } set { _dices = value; } }
    public void DicesAdd(DiceEnum dice) { _dices.Add(dice); }

}
