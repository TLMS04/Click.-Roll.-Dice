using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Globalization;

public class Game : MonoBehaviour
{
    private static GameData _gameData;
    /*“ут можно было через Zenject и не прокидывать €вную ссылку*/
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private long _costUpgradeDice;

    [SerializeField] private uint _maxCountDices = 20;
    public static UnityEvent<long> ScoreChanged = new UnityEvent<long>();
    public static UnityEvent<ushort> BonusScoreChanged = new UnityEvent<ushort>();
    private void Awake()
    {
        Save.Load();
    }
    void Start()
    {
        _gameData = GameData.GetInstance();
        _gameUI.UpgradeDiceButtonClick.AddListener(UpgradeDice);
        ScoringAfk();
        //_gameUI.HoursAfkFarmButtonClick.AddListener();
        //_gameUI.UpgradeHoursAfkFarmButtonClick.AddListener();
    }
    void OnDisable()
    {
        if (_gameData.SecondsAfkFarm == 0)
        {
            _gameData.SecondsAfkFarm = Dice.RollDice(_gameData.DiceAfkFarm) * 60 * 60;
        }
        Save.SaveData();
    }

    public static void IncreaseScore()
    {
        int sum = 0;
        ushort bouns = _gameData.BonusScoreToRoll;
        foreach (var item in _gameData.Dices)
        {
            var resultRoll = Dice.RollDice(item) + bouns;
            sum += resultRoll;
            Debug.LogError($"{item.ToString()} = {resultRoll}");
        }
        Debug.LogError(sum);
        _gameData.Score += sum;
        ScoreChanged?.Invoke(_gameData.Score);


        Save.SaveData();
    }
    private void ScoringAfk()
    {
        if (_gameData.LastVisit == null)
        {
            return;
        }
        DateTime lastVisit = DateTime.ParseExact(_gameData.LastVisit, "u", CultureInfo.InvariantCulture);
        TimeSpan timePassed = DateTime.UtcNow - lastVisit;
        int secondsPassed = (int)timePassed.TotalSeconds;
        secondsPassed = Mathf.Clamp(secondsPassed, 0, _gameData.SecondsAfkFarm);
        float sumAverageDicesValue = 0;
        foreach (var item in _gameData.Dices)
        {
            sumAverageDicesValue += (((float)item) / 2) + 0.5f + _gameData.BonusScoreToRoll;
            Debug.Log(sumAverageDicesValue);
        }
        Debug.Log(secondsPassed);
        int score = Mathf.RoundToInt(sumAverageDicesValue * secondsPassed);
        Debug.Log(score);
        _gameData.Score += score;
        if(secondsPassed == _gameData.SecondsAfkFarm)
        {
            _gameData.SecondsAfkFarm = 0;
        }
        else
        {
            _gameData.SecondsAfkFarm -= secondsPassed;
        }
        ScoreChanged?.Invoke(_gameData.Score);
    }
    private void UpgradeDice()
    {
        var actualCostUpgradeDice = _costUpgradeDice * _gameData.CountUpgrade;
        if (actualCostUpgradeDice <= _gameData.Score)
        {
            _gameData.Score -= actualCostUpgradeDice;
            List<DiceEnum> dices = _gameData.Dices;
            int countDice = dices.Count;
            var diceEnumArray = Enum.GetValues(typeof(DiceEnum)).Cast<DiceEnum>().ToArray();
            if (dices[countDice - 1] == DiceEnum.d20 && countDice < _maxCountDices)
            {
                dices.Add(DiceEnum.d4);
            }
            else if (countDice <= _maxCountDices && dices[countDice - 1] != DiceEnum.d20)
            {
                dices[countDice - 1] = diceEnumArray[Array.BinarySearch(diceEnumArray, dices[countDice - 1]) + 1];
            }
            else
            {
                _gameData.BonusScoreToRoll++;
                BonusScoreChanged?.Invoke(_gameData.BonusScoreToRoll);
            }
            _gameData.CountUpgrade++;
            _gameData.Dices = dices;
            ScoreChanged?.Invoke(_gameData.Score);
        }
    }
}
