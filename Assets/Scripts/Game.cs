using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Game : MonoBehaviour
{
    private static GameData _gameData;

    public static UnityEvent<long> ScoreChanged = new UnityEvent<long>();
    void Start()
    {
        _gameData = GameData.GetInstance();
    }

    public static void IncreaseScore()
    {
        int sum = 0;
        foreach (var item in _gameData.Dices)
        {
            sum += Dice.RollDice(item);
        }
        Debug.LogError(sum);
        _gameData.Score += sum;
        ScoreChanged?.Invoke(_gameData.Score);
    }
}
