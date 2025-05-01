using Godot;
using System;

public partial class Player : Node
{
    #region PLAYER VARIABLES
    // Permanent variables
    [Export]
    public int StartingMaxEnergy = 3;
    [Export]
    private int StartingMaxRage = 50;
    [Export]
    private int StartingMaxHandDrawSize = 5;

    // Changeable variables
    public int maxEnergy { get; set; }
    public int rage { get; set; }
    public int maxRage { get; set; }
    public int maxHandDrawSize { get; set; }
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
        base._Ready();
    }

    public void InitializePlayer()
    {
        ResetModifiers();
        maxRage = StartingMaxRage;
        maxEnergy = StartingMaxEnergy;
        maxHandDrawSize = StartingMaxHandDrawSize;
        rage = 0;
        UpdateLabels();
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
        if(rage < 0) rage = 0;
        if (rage >= maxRage)
        {
            rage = 0;
            EmitSignal(SignalName.GameOver);
        }
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        _rageSlider.MaxValue = maxRage;
        _rageSlider.Value = rage;
        _rageLabel.Text = $"Rage: {rage}/{maxRage}";
        double temp = maxRage / 10;
        _rageSlider.TickCount = (int)Math.Floor(temp);
    }
}
