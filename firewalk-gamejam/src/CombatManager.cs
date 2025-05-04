using Godot;
using System;
using System.Collections.Generic;

public partial class CombatManager : Node
{

    [Signal]
    public delegate void CardSelectCallEventHandler(Card cardSelection1, Card cardSelection2, Card cardSelection3);

    #region COMBAT SCREEN NODES
    private CardManager _cardManager;
    public Player _player;
    private Enemy _enemy;

    private Label _turnLabel;
    private Label _attackLabel;
    private Label _resistanceLable;
    #endregion

    public int Level = 1;

    private int _currentTurn = 1;

    public override void _Ready()
    {
        _cardManager = GetNode<CardManager>("Card Manager");
        _player = GetNode<Player>("Player");
        _enemy = GetNode<Enemy>("Enemy");
        _turnLabel = GetNode<Label>("Combat UI/Turn Label");
        _attackLabel = GetNode<Label>("Combat UI/Attack Label");
        _resistanceLable = GetNode<Label>("Combat UI/Resistance Label");

        _cardManager.CardPlayed += ResolveCard;
        _player.StatsChanged += PlayerStatsChangedHandler;

        _enemy.EnemyDiedSignal += EnemyDiedHandler;

        ResetGame();
        base._Ready();
    }

    public void NewGameStarted()
    {
        ResetGame();
        NewLevel();
    }

    public void CardSkippedHandler()
    {
        NewLevel();
    }

    public void CardSelectedHandler(Card card = null)
    {
        _cardManager.cardsPlayerOwns.Add(card);
        NewLevel();
    }

    public void NewLevel()
    {
        if (Level % 5 == 0)
        {
            _enemy.NewEnemy(Level, EnemyResource.EnemyTags.BossEnemy);
        }
        else
        {
            _enemy.NewEnemy(Level, EnemyResource.EnemyTags.EasyEnemy);
        }
        _enemy.ChangeIntent(_currentTurn);
        _cardManager.maxEnergy = _player.maxEnergy;
        _cardManager.ResetEnergy();
        _currentTurn = 1;
        _cardManager.ResetDeck();
        _player.ResetModifiers();
        UpdatePlayerStatsLabels();
        ResetHand();
        _turnLabel.Text = $"Turn: {_currentTurn}";
        Level++;
    }

    public void EnemyDiedHandler()
    {
        _cardManager.playerHand.DiscardAllCards();

        List<Card> temp = new List<Card>();
        List<Card> sendTemp = new List<Card>();
        Random rnd = new Random();
        foreach (CardResource cardResource in _cardManager.cardsResourcesList)
        {
            Card tempCard = (Card)_cardManager.cardBase.Instantiate();
            tempCard.cardResource = cardResource;
            tempCard.Name = cardResource.CardName;
            temp.Add(tempCard);
        }
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = rnd.Next(temp.Count);
            Card tempCard = (Card)_cardManager.cardBase.Instantiate();
            tempCard.cardResource = temp[randomIndex].cardResource;
            sendTemp.Add(tempCard);
            temp.RemoveAt(randomIndex);
        }
        temp.Clear();
        EmitSignal(SignalName.CardSelectCall, sendTemp[0], sendTemp[1], sendTemp[2]);
        sendTemp.Clear();
    }

    private void PlayerStatsChangedHandler()
    {
        if (_player.maxEnergy != _cardManager.maxEnergy)
        {
            _cardManager.maxEnergy = _player.maxEnergy;
            _cardManager.UpdateLabels();
        }
        UpdatePlayerStatsLabels();
    }

    private void ResolveCard(Card card)
    {
        // TO DO: Make this take into account buffs and debuffs
        int i;
        if (card.cardResource.DamageValue != 0)
        {
            i = card.cardResource.DamageValue + _player.attack;
            _enemy.TakeDamage(i);
        }
        if(card.cardResource.AttackValue != 0)
        {
            _player.AddAttack(card.cardResource.AttackValue);
            UpdatePlayerStatsLabels();
        }
        if (card.cardResource.ResistanceValue != 0)
        {
            _player.AddResistance(card.cardResource.ResistanceValue);
            _enemy.UpdateEnemyIntent(_player.resistance);
            UpdatePlayerStatsLabels();
        }
        if (card.cardResource.RageValue != 0)
        {
            _player.IncreaseRage(card.cardResource.RageValue);
        }
    }

    public void ResolveEnemyIntent()
    {
        EnemyIntents currentIntent;
        currentIntent = _enemy.currentEnemyIntent;
        switch (_enemy.currentEnemyIntent.intentsType)
        {
            case EnemyIntents.IntentsType.Rage:
                _player.IncreaseRage(currentIntent.value);
                break;
            case EnemyIntents.IntentsType.AtkDebuff:
                _player.AddAttack(currentIntent.value);
                UpdatePlayerStatsLabels();
                break;
            case EnemyIntents.IntentsType.Heal:
                _enemy.Heal(currentIntent.value);
                break;
            default:
                break;
        }
    }

    public void ResetGame()
    {
        _player.InitializePlayer();
        _cardManager.maxEnergy = _player.maxEnergy;
        _cardManager.ResetEnergy();
        _cardManager.GenerateStartingDeck();
        _currentTurn = 0;
    }

    private void NextTurn()
    {
        _currentTurn++;
        DecreasePlayerModifiers();
        ResetHand();
        ResolveEnemyIntent();
        _enemy.ChangeIntent(_currentTurn);
        _turnLabel.Text = $"Turn: {_currentTurn}";
    }

    private void DecreasePlayerModifiers()
    {
        if (_player.attack-1 >= _player.rageTicks) { _player.attack--; }
        _player.resistance = 0;
        UpdatePlayerStatsLabels();
    }

    private void ResetHand()
    {
        _cardManager.ResetEnergy();
        _cardManager.playerHand.DiscardAllCards();
        _cardManager.AddCardToHand(_player.maxHandDrawSize);
    }

    private void UpdatePlayerStatsLabels()
    {
        _attackLabel.Text = $"Atk: {_player.attack}";
        _resistanceLable.Text = $"Res: {_player.resistance}";
        _cardManager.playerHand.UpdateAttackCardsInHand(_player.attack);
    }
}
