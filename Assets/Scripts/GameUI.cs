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

    void Start()
    {
        _gameData = GameData.GetInstance();
        Game.ScoreChanged.AddListener(OnScoreChanged);
        _tabZone.GetComponent<SpriteRenderer>().sprite = _spritesDice[(int)_gameData.Dices[0]];
    }

    public void OnScoreChanged(long score)
    {
        _scoreText.text = score.ToString();
    }
}
