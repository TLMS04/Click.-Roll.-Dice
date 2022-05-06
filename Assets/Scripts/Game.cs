using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class Game : MonoBehaviour
{
    private static GameData _gameData;
    /*“ут можно было через Zenject и не прокидывать €вную ссылку*/
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private long _costUpgradeDice;

    [SerializeField] private uint _maxCountDices = 20;
    public static UnityEvent<long> ScoreChanged = new UnityEvent<long>();
    public static UnityEvent<ushort> BonusScoreChanged = new UnityEvent<ushort>();

    void Start()
    {
        _gameData = GameData.GetInstance();
        _gameUI.UpgradeDiceButtonClick.AddListener(UpgradeDice);
        //_gameUI.HoursAfkFarmButtonClick.AddListener();
        //_gameUI.UpgradeHoursAfkFarmButtonClick.AddListener();
    }
    void OnApplicationPause()
    {
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
