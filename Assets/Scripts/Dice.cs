using System;
using System.Collections.Generic;

[Serializable]
public enum DiceEnum
{
    d4 = 4,
    d6 = 6,
    d8 = 8,
    d10 = 10,
    d12 = 12,
    d20 = 20
}
public static class Dice
{
    public static int RollDice(DiceEnum diceEnum)
    {
        return UnityEngine.Random.Range(1, (int)diceEnum + 1);
    }
    public static List<int> RollDice(DiceEnum diceEnum, uint d = 1)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < d; i++)
        {
            result.Add(UnityEngine.Random.Range(1, (int)diceEnum + 1 ));
        }
        return result;
    }
}