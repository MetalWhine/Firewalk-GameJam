using Godot;
using System;

[GlobalClass]
public partial class CardResource : Resource
{
    #region CARDPROPERTIES
    [Export]
    public string CardName { get; set; }

    [Export]
    public int CardCost { get; set; }

    [Export]
    public CardType cardType { get; set; }

    [Export]
    public Texture2D CardSrpite { get; set; }

    // Damage to actually hit the opponent
    [Export]
    public int DamageValue { get; set; }

    // Modifiers of damage
    [Export]
    public int AttackValue { get; set; }

    // Modifiers of resistance
    [Export]
    public int ResistanceValue { get; set; }
    
    // Modifiers of rage
    [Export]
    public int RageValue { get; set; }

    // Describes cards
    [Export]
    public string CardDescription { get; set; }
    #endregion

    public enum CardType
    {
        Attack,
        Resistance,
        Rage,
    }
}
