// TO DO
// ADD FUNCTIONALITY FOR RAGE METER
// EVERY 10 RAGE = 1 EXTRA ATTACK
// EVERY 30 RAGE = 1 EXTRA ENERGY

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
    public int rageTicks { get; set; }
    #endregion

    #region CARD MODIFIERES
    public int attack { get; set; }
    public int resistance { get; set; }
    #endregion

    #region DELEGATES AND SIGNALS
    [Signal]
    public delegate void GameOverEventHandler();
    [Signal]
    public delegate void StatsChangedEventHandler();
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
        attack = rageTicks;
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

        int newRageTicks = Mathf.FloorToInt(rage/10);

        if (newRageTicks > rageTicks) { 
            AddAttack(newRageTicks-rageTicks);
            if (newRageTicks % 3 == 0)
            {
                AddMaxEnergy(1);
            }
            EmitSignal(SignalName.StatsChanged);
        }
        else if (newRageTicks < rageTicks)
        {
            AddAttack(rageTicks - newRageTicks);
            if (rageTicks % 3 == 0)
            {
                AddMaxEnergy(-1);
            }
            EmitSignal(SignalName.StatsChanged);
        }
        rageTicks = newRageTicks;

        if (rage >= maxRage)
        {
            rage = 0;
            EmitSignal(SignalName.GameOver);
        }
        UpdateLabels();
    }

    private void AddMaxEnergy(int i)
    {
        maxEnergy += i;
    }

    private void UpdateLabels()
    {
        _rageSlider.MaxValue = maxRage;
        _rageSlider.Value = rage;
        _rageLabel.Text = $"Rage: {rage}/{maxRage}";
        double temp = maxRage / 10;
        _rageSlider.TickCount = (int)Math.Floor(temp)+1;
    }
}
