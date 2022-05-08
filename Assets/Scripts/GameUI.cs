using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject _tabZone;
    
    [SerializeField] private List<Sprite> _spritesDice;
    [SerializeField] private List<Sprite> _spritesDiceUpgrade;
    [SerializeField] private List<Sprite> _spritesDiceUpgradeAfk;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _bounsScoreText;
    [SerializeField] private Text _costDiceUpgradeText;
    [SerializeField] private Text _costDiceUpgradeAfkText;


    [SerializeField] private Button _upgradeDiceButton;
    [SerializeField] private Button _upgradeDiceAfkFarmButton;


    private GameData _gameData;
    private Dictionary<int, Sprite> _spritesDiceDictionary;
    private Dictionary<int, Sprite> _spritesDiceUpgradeDictionary;
    private Dictionary<int, Sprite> _spritesDiceUpgradeAfkDictionary;

    public UnityEvent UpgradeDiceButtonClick = new UnityEvent();
    public UnityEvent UpgradeDiceAfkFarmButtonClick = new UnityEvent();
    private void Awake()
    {
        Save.ScoreChanged.AddListener(OnScoreChanged);
    }
    void Start()
    {
        Game.ScoreChanged.AddListener(OnScoreChanged);
        Game.BonusScoreChanged.AddListener(OnBonusScoreChanged);
        Game.DiceAfkChanged.AddListener(OnDiceUpgradeAfkChanged);
        Game.CostUpgradeDiceChanged.AddListener(OnCostUpgradeDiceChanged);
        Game.CostUpgradeDiceAfkChanged.AddListener(OnCostUpgradeDiceAfkChanged);
        SetSpritesDices();

        _upgradeDiceButton.onClick.AddListener(OnUpgradeDiceButtonClick);
        _upgradeDiceButton.onClick.AddListener(OnDiceChanged);
        _upgradeDiceButton.onClick.AddListener(OnDiceUpgradeChanged);

        _upgradeDiceAfkFarmButton.onClick.AddListener(OnUpgradeDiceAfkFarmButtonClick);
        _upgradeDiceAfkFarmButton.onClick.AddListener(OnDiceUpgradeAfkChanged);
    }
    private void InitDictionaries()
    {
        _spritesDiceDictionary = new Dictionary<int, Sprite>();
        _spritesDiceUpgradeDictionary = new Dictionary<int, Sprite>();
        _spritesDiceUpgradeAfkDictionary = new Dictionary<int, Sprite>();
        List<int> diceNumber = new List<int>() { 4, 6, 8, 10, 12, 20 };
        for (int i = 0; i < _spritesDice.Count; i++)
        {
            _spritesDiceDictionary.Add(diceNumber[i], _spritesDice[i]);
        }
        for (int i = 0; i < _spritesDiceUpgrade.Count; i++)
        {
            _spritesDiceUpgradeDictionary.Add(diceNumber[i], _spritesDiceUpgrade[i]);
        }
        for (int i = 0; i < _spritesDiceUpgradeAfk.Count; i++)
        {
            _spritesDiceUpgradeAfkDictionary.Add(diceNumber[i], _spritesDiceUpgradeAfk[i]);
        }

    }
    private void SetSpritesDices()
    {
        InitDictionaries();
        _gameData = GameData.GetInstance();
        OnDiceChanged();
        OnDiceUpgradeChanged();
        OnDiceUpgradeAfkChanged();
    }
    public void OnScoreChanged(long score)
    {
        _scoreText.text = score.ToString();
    }
    public void OnBonusScoreChanged(ushort bonusScore)
    {
        _bounsScoreText.text = $"+{bonusScore}";
    }
    public void OnCostUpgradeDiceChanged(long cost)
    {
        _costDiceUpgradeText.text = $"{cost}";
    }
    public void OnCostUpgradeDiceAfkChanged(long cost)
    {
        _costDiceUpgradeAfkText.text = $"{cost}";
    }
    public void OnDiceChanged()
    {
        _tabZone.GetComponent<SpriteRenderer>().sprite = _spritesDiceDictionary[(int)_gameData.Dices[0]];
    }
    public void OnDiceUpgradeChanged()
    {
        _upgradeDiceButton.GetComponent<Image>().sprite = _spritesDiceUpgradeDictionary[(int)_gameData.Dices[_gameData.Dices.Count-1]];
    }
    public void OnDiceUpgradeAfkChanged()
    {
        _upgradeDiceAfkFarmButton.GetComponent<Image>().sprite = _spritesDiceUpgradeAfkDictionary[(int)_gameData.DiceAfkFarm];
    }
    private void OnUpgradeDiceButtonClick()
    {
        UpgradeDiceButtonClick?.Invoke();
    }
    private void OnUpgradeDiceAfkFarmButtonClick()
    {
        UpgradeDiceAfkFarmButtonClick?.Invoke();
    }

}
