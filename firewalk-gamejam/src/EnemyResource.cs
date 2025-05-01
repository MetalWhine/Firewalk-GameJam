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
    public int EnemyPower { get; set; }
    [Export]
    public EnemyIntents[] EnemyIntentsArray;
    [Export]
    public Texture2D EnemySprite { get; set; }
    [Export]
    public EnemyTags enemyTag { get; set; }

    public enum EnemyTags
    {
        EasyEnemy,
        MediumEnemy,
        HardEnemy,
        BossEnemy,
    }

    public EnemyIntents SetNextEnemyIntent(int turnCount)
    {
        EnemyIntents nextIntent;
        int i;

        i = (turnCount-1) % EnemyIntentsArray.Length;
        nextIntent = EnemyIntentsArray[i];
        return nextIntent;
    }
}
