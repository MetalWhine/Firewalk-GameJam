using Godot;
using System;

public partial class Player : Node
{
    private const int StartingMaxEnergy = 3;
    private const int StartingMaxRage = 50;

    #region PLAYER VARIABLES
    public int currentEnergy { get; set; }
    public int maxEnergy { get; set; } = StartingMaxEnergy;
    public int rage { get; set; } = 0;
    public int maxRage { get; set; } = StartingMaxEnergy;
    #endregion

    #region CARD MODIFIERES
    public int attack { get; set; }
    public int resistance { get; set; }
    #endregion

    #region DELEGATES AND SIGNALS
    [Signal]
    public delegate void GameOverEventHandler();
    #endregion

    #region SPRITES AND UI
    private Label _rageLabel;
    private HSlider _rageSlider;
    #endregion

    public override void _Ready()
    {
        _rageLabel = GetNode<Label>("Rage Label");
        _rageSlider = GetNode<HSlider>("Rage Meter");
        InitializePlayer();
        base._Ready();
    }

    public void InitializePlayer()
    {
        maxEnergy = StartingMaxEnergy;
        ResetEnergy();
        ResetModifiers();
        maxRage = StartingMaxRage;
        rage = 0;
        _rageSlider.MaxValue = maxRage;
        _rageSlider.Value = rage;
        _rageLabel.Text = $"Rage: {rage}/{maxRage}";
        double temp = maxRage / 10;
        _rageSlider.TickCount = (int)Math.Floor(temp);
    }

    public void ResetEnergy() {         
        currentEnergy = maxEnergy;
    }

    public void ResetModifiers()
    {
       attack = 0;
        resistance = 0;
    }

    public void AddAttack(int attackIncreaseAmount)
    {
        attack += attackIncreaseAmount;
    }

    public void AddResistance(int resistanceIncreaseAmount)
    {
        resistance += resistanceIncreaseAmount;
    }

    public void IncreaseRage(int rageIncreaseAmount)
    {
        rage += rageIncreaseAmount;
        if (rage >= maxRage)
        {
            rage = 0;
            EmitSignal(SignalName.GameOver);
        }
    }
}
