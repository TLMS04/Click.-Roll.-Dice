using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _tabZone;
    [SerializeField] private List<Sprite> _spritesDice;
    [SerializeField] private Text _scoreText;


    private GameData _gameData;
    private Dictionary<int, Sprite> _spritesDiceDictionaty;
    void Start()
    {
        _spritesDiceDictionaty = new Dictionary<int, Sprite>();
        List<int> diceNumber = new List<int>() { 4, 6, 8, 10, 12, 20 }; 
        for (int i = 0; i < _spritesDice.Count; i++)
        {
            _spritesDiceDictionaty.Add(diceNumber[i], _spritesDice[i]);
        }
        _gameData = GameData.GetInstance();
        Game.ScoreChanged.AddListener(OnScoreChanged);
        _tabZone.GetComponent<SpriteRenderer>().sprite = _spritesDiceDictionaty[(int)_gameData.Dices[0]];
    }

    public void OnScoreChanged(long score)
    {
        _scoreText.text = score.ToString();
    }
}
