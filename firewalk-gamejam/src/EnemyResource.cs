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
        EliteEnemy,
        BossEnemy,
    }

    public EnemyIntents SetNextEnemyIntent(int turnCount, int enemyPower)
    {
        EnemyIntents nextIntent;
        int i;
        if (turnCount == 0)
        {
            i = 0;
        }
        else
        {
            i = (turnCount - 1) % EnemyIntentsArray.Length;
        }
        nextIntent = EnemyIntentsArray[i];
        nextIntent.CalculateValue(enemyPower);
        return nextIntent;
    }
}
