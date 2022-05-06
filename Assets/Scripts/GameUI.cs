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
    [SerializeField] private Text _bounsScoreText;

    [SerializeField] private Button _upgradeDiceButton;
    [SerializeField] private Button _hoursAfkFarmButton;


    private GameData _gameData;
    private Dictionary<int, Sprite> _spritesDiceDictionaty;

    public UnityEvent UpgradeDiceButtonClick = new UnityEvent();
    public UnityEvent HoursAfkFarmButtonClick = new UnityEvent();
    public UnityEvent UpgradeHoursAfkFarmButtonClick = new UnityEvent();
    void Start()
    {
        Save.ScoreChanged.AddListener(OnScoreChanged);
        Save.Load();
        Game.ScoreChanged.AddListener(OnScoreChanged);
        Game.BonusScoreChanged.AddListener(OnBonusScoreChanged);
        SetSpriteDice();

        _upgradeDiceButton.onClick.AddListener(OnUpgradeDiceButtonClick);
        _upgradeDiceButton.onClick.AddListener(OnDiceChanged);

        _hoursAfkFarmButton.onClick.AddListener(OnHoursAfkFarmButtonClick);

    }
    private void SetSpriteDice()
    {
        _spritesDiceDictionaty = new Dictionary<int, Sprite>();
        List<int> diceNumber = new List<int>() { 4, 6, 8, 10, 12, 20 };
        for (int i = 0; i < _spritesDice.Count; i++)
        {
            _spritesDiceDictionaty.Add(diceNumber[i], _spritesDice[i]);
        }
        _gameData = GameData.GetInstance();
        _tabZone.GetComponent<SpriteRenderer>().sprite = _spritesDiceDictionaty[(int)_gameData.Dices[0]];
    }
    public void OnScoreChanged(long score)
    {
        _scoreText.text = score.ToString();
    }
    public void OnBonusScoreChanged(ushort bonusScore)
    {
        _bounsScoreText.text = $"+{bonusScore}";
    }
    public void OnDiceChanged()
    {
        _tabZone.GetComponent<SpriteRenderer>().sprite = _spritesDiceDictionaty[(int)_gameData.Dices[0]];
    }
    private void OnUpgradeDiceButtonClick()
    {
        UpgradeDiceButtonClick?.Invoke();
    }
    private void OnHoursAfkFarmButtonClick()
    {
        HoursAfkFarmButtonClick?.Invoke();
    }

}
