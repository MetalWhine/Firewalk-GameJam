using Godot;
using System;

public partial class CombatManager : Node
{
    #region COMBAT SCREEN NODES
    private CardManager _cardManager;
    private Player _player;
    private Enemy _enemy;

    private Label _turnLabel;
    private Label _attackLabel;
    private Label _resistanceLable;
    #endregion

    public int Level = 1;

    private int _currentTurn = 0;

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_accept"))
        {
            _enemy.NewEnemy(Level, EnemyResource.EnemyTags.EasyEnemy);
            Level++;
        }
        base._Process(delta);
    }

    public override void _Ready()
    {
        _cardManager = GetNode<CardManager>("Card Manager");
        _player = GetNode<Player>("Player");
        _enemy = GetNode<Enemy>("Enemy");
        _turnLabel = GetNode<Label>("Turn Label");
        _attackLabel = GetNode<Label>("Attack Label");
        _resistanceLable = GetNode<Label>("Resistance Label");

        _cardManager.CardPlayed += ResolveCard;

        ResetGame();
        base._Ready();
    }

    private void ResolveCard(Card card)
    {
        if (card.cardResource.DamageValue != 0)
        {
            _enemy.TakeDamage(card.cardResource.DamageValue);
        }
        if(card.cardResource.AttackValue != 0)
        {
            _player.AddAttack(card.cardResource.AttackValue);
            UpdatePlayerStatsLabels();
        }
        if (card.cardResource.ResistanceValue != 0)
        {
            _player.AddResistance(card.cardResource.ResistanceValue);
            UpdatePlayerStatsLabels();
        }
        if (card.cardResource.RageValue != 0)
        {
            _player.IncreaseRage(card.cardResource.RageValue);
        }
    }

    private void ResetGame()
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
        _turnLabel.Text = $"Turn: {_currentTurn}";
    }

    private void DecreasePlayerModifiers()
    {
        if (_player.attack > 0) { _player.attack--; }
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
    }
}
