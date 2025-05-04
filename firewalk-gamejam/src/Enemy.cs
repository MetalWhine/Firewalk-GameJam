using Godot;
using System;
using System.Collections.Generic;

public partial class Enemy : Node2D
{
    #region ENEMY NODES
    private Sprite2D _enemySprite;
    private HSlider _healthSlider;
    private Label _nameLabel;
    private Label _healthLabel;
    private Label _intentLabel;
    #endregion

    #region ENEMY STATS
    [Export]
    public EnemyResource[] possibleEnemyResources;
    public EnemyResource enemyResource;
    public EnemyIntents currentEnemyIntent;
    private int _currentHealth;
    private int _maxHealth;
    private float _levelMultiplier;
    private int _enemyPower;
    #endregion

    [Signal]
    public delegate void EnemyDiedSignalEventHandler();

    public override void _Ready()
    {
        _enemySprite = GetNode<Sprite2D>("Sprite2D");
        _healthSlider = GetNode<HSlider>("Health Meter");
        _nameLabel = GetNode<Label>("Name Label");
        _healthLabel = GetNode<Label>("Health Label");
        _intentLabel = GetNode<Label>("Intent Label");
        base._Ready();
    }

    public void NewEnemy(int _currentLevel,EnemyResource.EnemyTags tags)
    {
        GetRandomEnemy(tags);
        _levelMultiplier = 1 + (_currentLevel * 0.025f);
        InitializeEnemy();
    }

    private void EnemyDied()
    {
        EmitSignal(SignalName.EnemyDiedSignal);
    }

    public void TakeDamage(int i)
    {
        _currentHealth -= i;
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            EnemyDied();
        }
        UpdateLabels();
    }

    public void Heal(int i)
    {
        _currentHealth += i;
        if(_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
        UpdateLabels();
    }

    private void GetRandomEnemy(EnemyResource.EnemyTags tags)
    {
        List<EnemyResource> candidates = new List<EnemyResource>();
        if (possibleEnemyResources.Length > 0)
        {
            foreach(EnemyResource enemy in possibleEnemyResources)
            {
                if(enemy.enemyTag == tags)
                {
                    candidates.Add(enemy);
                }
            }
            if (candidates.Count > 0)
            {
                Random rnd = new Random();
                int i = rnd.Next(0, candidates.Count);
                enemyResource = candidates[i];
                InitializeEnemy();
            }
            else
            {
                GD.PrintErr($"No enemies of {tags.ToString()} exists in possible enemy resources located in the Enemy!");
            }
        }
    }

    public void InitializeEnemy()
    {
        _nameLabel.Text = enemyResource.EnemyName;
        _enemySprite.Texture = enemyResource.EnemySprite;
        _enemyPower = (int)Mathf.Floor(MathF.Pow(enemyResource.EnemyPower, _levelMultiplier));
        _maxHealth = (int)Mathf.Floor(MathF.Pow(enemyResource.MaxEnemyHealth, _levelMultiplier));
        _currentHealth = _maxHealth;
        currentEnemyIntent = enemyResource.SetNextEnemyIntent(0, _enemyPower);
        UpdateLabels();
    }

    public void UpdateLabels()
    {
        _healthLabel.Text = $"Health: {_currentHealth}/{_maxHealth}";
        _intentLabel.Text = $"Intent: {currentEnemyIntent.GenerateIntentText()}";
        _healthSlider.MaxValue = _maxHealth;
        _healthSlider.Value = _currentHealth;
    }

    public void ChangeIntent(int currentTurn, int playerRes = 0)
    {
        currentEnemyIntent = enemyResource.SetNextEnemyIntent(currentTurn, _enemyPower);
        UpdateLabels();
    }

    public void UpdateEnemyIntent(int playerRes)
    {
        currentEnemyIntent.CalculateValue(_enemyPower, playerRes);
        UpdateLabels();
    }
}
