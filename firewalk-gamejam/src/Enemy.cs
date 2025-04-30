using Godot;
using System;

public partial class Enemy : Node2D
{
    #region ENEMY NODES
    private Sprite2D _enemySprite;
    private HSlider _healthSlider;
    private Label _nameLabel;
    private Label _healthLabel;
    private Label _intentLabel;
    #endregion

    #region ENEMY SETTINGS
    public EnemyResource enemyResource;
    #endregion
}
