using Godot;
using System;

[GlobalClass]
public partial class EnemyResource : Resource
{
    [Export]
    public string EnemyName { get; set; }
    [Export]
    public int MaxEnemyHealth { get; set; }
    [Export]
    public EnemyIntents[] enemyIntentsArray;
}
