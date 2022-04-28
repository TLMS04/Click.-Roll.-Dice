using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameData
{
    private static GameData _instance;

    private long _score;
    private ushort _bonusScoreToRoll;
    private List<DiceEnum> _dices;
    private DiceEnum _diceAfkfarm;
    private ushort _hoursAfkFarm;

    private GameData() { }
    private GameData(long score, ushort bonusScoreToRoll, ushort hoursAfkFarm, DiceEnum diceAfkfarm, List<DiceEnum> dices) 
    {
        _score = score;
        _bonusScoreToRoll = bonusScoreToRoll;
        _hoursAfkFarm = hoursAfkFarm;
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
            _instance.Dices = new List<DiceEnum>();
            _instance.DicesAdd(DiceEnum.d4);
            _instance.HoursAfkFarm = 0;
            _instance.DiceAfkfarm = DiceEnum.d4;
        }
            
        return _instance;
    }
    public static GameData GetInstance(long score, ushort bonusScoreToRoll, ushort hoursAfkFarm, DiceEnum diceAfkfarm, List<DiceEnum> dices)
    {
        if (_instance == null)
            _instance = new GameData(score, bonusScoreToRoll, hoursAfkFarm, diceAfkfarm,  dices);
        return _instance;
    }

    public long Score { get { return _score; } set { _score = value; } }
    public ushort BonusScoreToRoll { get { return _bonusScoreToRoll; } set { _bonusScoreToRoll = value; } }
    public ushort HoursAfkFarm { get { return _hoursAfkFarm; } set { _hoursAfkFarm = value; } }
    public DiceEnum DiceAfkfarm { get { return _diceAfkfarm; } set { _diceAfkfarm = value; } }
    public List<DiceEnum> Dices { get { return _dices; } set { _dices = value; } }
    public void DicesAdd(DiceEnum dice) { _dices.Add(dice); }

}
