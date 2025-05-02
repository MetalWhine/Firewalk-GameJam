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

    private int _currentTurn = 1;

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_accept"))
        {
            NewLevel();
        }
        base._Process(delta);
    }

    public void NewLevel()
    {
        _enemy.NewEnemy(Level, EnemyResource.EnemyTags.EasyEnemy);
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

    public override void _Ready()
    {
        _cardManager = GetNode<CardManager>("Card Manager");
        _player = GetNode<Player>("Player");
        _enemy = GetNode<Enemy>("Enemy");
        _turnLabel = GetNode<Label>("Combat UI/Turn Label");
        _attackLabel = GetNode<Label>("Combat UI/Attack Label");
        _resistanceLable = GetNode<Label>("Combat UI/Resistance Label");

        _cardManager.CardPlayed += ResolveCard;

        ResetGame();
        base._Ready();
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
        ResolveEnemyIntent();
        _enemy.ChangeIntent(_currentTurn);
        _turnLabel.Text = $"Turn: {_currentTurn}";
    }

    private void DecreasePlayerModifiers()
    {
        if (_player.attack-1 >= 0) { _player.attack--; }
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
