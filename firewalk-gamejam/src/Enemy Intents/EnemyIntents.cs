using Godot;
using System;

[GlobalClass]
public partial class EnemyIntents : Resource
{
    [Export]
    public IntentsType intentsType { get; set; }
    [Export]
    private float OptionalMultiplier { get; set; } = 1;
    [Export]
    private int OptionalAddition { get; set; } = 0;

    public int value;

    public void CalculateValue(int enemyPower, int playerRes=0)
    {
        switch (intentsType)
        {
            case IntentsType.Rage:
                value = (int)((enemyPower + OptionalAddition) * OptionalMultiplier)-playerRes;
                if (value < 0)
                {
                    value = 0;
                }
                break;
            case IntentsType.AtkDebuff:
                value = (int)((enemyPower + OptionalAddition) * OptionalMultiplier)*-1;
                break;
            default:
                value = (int)((enemyPower + OptionalAddition) * OptionalMultiplier);
                if (value < 0)
                {
                    value = 0;
                }
                break;
        }

        
    }

    public string GenerateIntentText()
    {
        string intentText = "";
        string numberText;
        if (value >= 0){ numberText = $"+{value}"; }
        else{ numberText = value.ToString(); }
        switch (intentsType)
        {
            case IntentsType.Rage:
                intentText = numberText + " Rage";
                break;
            case IntentsType.AtkDebuff:
                intentText = numberText + " Attack";
                break;
            case IntentsType.Heal:
                intentText = numberText + " Heal";
                break;
            default:
                break;
        }
        return intentText;
    }

    public enum IntentsType
    {
        Rage,
        AtkDebuff,
        Heal,
    }
}
