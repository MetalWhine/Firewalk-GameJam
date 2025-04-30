using Godot;
using System;

public partial class CombatManager : Node
{
    #region COMBAT SCREEN NODES
    private CardManager _cardManager;
    private Player _player;
    #endregion

    private int _currentTurn = 0;

    public override void _Ready()
    {
        _cardManager = GetNode<CardManager>("Card Manager");
        _player = GetNode<Player>("Player");
        base._Ready();
    }
}
