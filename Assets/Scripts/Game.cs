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
    /*Тут можно было через Zenject и не прокидывать явную ссылку*/
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private long _costUpgradeDice;

    [SerializeField] private uint _maxCountDices = 20;
    public static UnityEvent<long> ScoreChanged = new UnityEvent<long>();
    public static UnityEvent<ushort> BonusScoreChanged = new UnityEvent<ushort>();
    public static UnityEvent DiceAfkChanged = new UnityEvent();

    public static UnityEvent<long> CostUpgradeDiceChanged = new UnityEvent<long>();
    public static UnityEvent<long> CostUpgradeDiceAfkChanged = new UnityEvent<long>();
    private void Awake()
    {
        Save.Load();
    }
    void Start()
    {
        _gameData = GameData.GetInstance();
        _gameUI.UpgradeDiceButtonClick.AddListener(UpgradeDice);
       _gameUI.UpgradeDiceAfkFarmButtonClick.AddListener(UpgradeDiceAfk);
        ScoringAfk();
        CostUpgradeDiceChanged?.Invoke(_costUpgradeDice * _gameData.CountUpgrade);
        CostUpgradeDiceAfkChanged?.Invoke(_costUpgradeDice * _gameData.CountUpgrade * _costUpgradeDice);
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
            var resultRoll = Dice.RollDice(item);
            if(resultRoll == 20) // Тут вызвать метод анимации критического успеха
            {
                resultRoll = 40;
            }else if(resultRoll == 1) // Тут вызвать метод анимации критического провала
            {
                resultRoll = -20;
            }
            sum += resultRoll + bouns;
            Debug.LogError($"{item.ToString()} = {resultRoll}");
        }
        Debug.LogError(sum);
        _gameData.Score += sum;
        ScoreChanged?.Invoke(_gameData.Score);
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
            CostUpgradeDiceChanged?.Invoke(_costUpgradeDice * _gameData.CountUpgrade);
            CostUpgradeDiceAfkChanged?.Invoke(_costUpgradeDice * _gameData.CountUpgrade * _costUpgradeDice);
        }
    }

    private void UpgradeDiceAfk()
    {
        
        var actualCostUpgradeDice = _costUpgradeDice * _gameData.CountUpgrade * _costUpgradeDice;
        if (actualCostUpgradeDice <= _gameData.Score)
        {
            _gameData.Score -= actualCostUpgradeDice;
            DiceEnum diceAfk = _gameData.DiceAfkFarm;
            var diceEnumArray = Enum.GetValues(typeof(DiceEnum)).Cast<DiceEnum>().ToArray();
            if (diceAfk == DiceEnum.d12)
            {
                return;
            }
            else if (diceAfk != DiceEnum.d12)
            {
                diceAfk = diceEnumArray[Array.BinarySearch(diceEnumArray, diceAfk) + 1];
            }
            _gameData.CountUpgrade++;
            _gameData.DiceAfkFarm = diceAfk;
            ScoreChanged?.Invoke(_gameData.Score);
            DiceAfkChanged?.Invoke();

            CostUpgradeDiceChanged?.Invoke(_costUpgradeDice * _gameData.CountUpgrade);
            CostUpgradeDiceAfkChanged?.Invoke(_costUpgradeDice * _gameData.CountUpgrade * _costUpgradeDice);
        }
    }
}
