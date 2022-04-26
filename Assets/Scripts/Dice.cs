using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public enum DiceEnum
{
    d4,
    d6,
    d8,
    d10,
    d12,
    d20,
    d100
}
public static class Dice
{
    public static int RollDice(DiceEnum diceEnum)
    {
        int result = 0;
        switch (diceEnum)
        {
            case DiceEnum.d4:
                result = UnityEngine.Random.Range(1, 5);
                break;
            case DiceEnum.d6:
                result = UnityEngine.Random.Range(1, 7);
                break;
            case DiceEnum.d8:
                result = UnityEngine.Random.Range(1, 9);
                break;
            case DiceEnum.d10:
                result = UnityEngine.Random.Range(1, 11);
                break;
            case DiceEnum.d12:
                result = UnityEngine.Random.Range(1, 13);
                break;
            case DiceEnum.d20:
                result = UnityEngine.Random.Range(1, 21);
                break;
            case DiceEnum.d100:
                result = UnityEngine.Random.Range(1, 101);
                break;
            default:
                result = 0;
                break;
        }
        return result;
    }
    public static List<int> RollDice(DiceEnum diceEnum, uint d = 1)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < d; i++)
        {
            switch (diceEnum)
            {
                case DiceEnum.d4:
                    result.Add(UnityEngine.Random.Range(1, 5));
                    break;
                case DiceEnum.d6:
                    result.Add(UnityEngine.Random.Range(1, 7));
                    break;
                case DiceEnum.d8:
                    result.Add(UnityEngine.Random.Range(1, 9));
                    break;
                case DiceEnum.d10:
                    result.Add(UnityEngine.Random.Range(1, 11));
                    break;
                case DiceEnum.d12:
                    result.Add(UnityEngine.Random.Range(1, 12));
                    break;
                case DiceEnum.d20:
                    result.Add(UnityEngine.Random.Range(1, 20));
                    break;
                case DiceEnum.d100:
                    result.Add(UnityEngine.Random.Range(1, 101));
                    break;
                default:
                    break;
            }
        }
        return result;
    }
}