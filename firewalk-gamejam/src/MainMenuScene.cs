using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class MainMenuScene : Control
{
    private Button _continueButton;

    public bool GamePlayed = false;

    [Signal]
    public delegate void NewGameSignalEventHandler();

    public override void _Ready()
    {
        _continueButton = GetNode<Button>("Continue Button");
        _continueButton.Disabled = true;
        base._Ready();
    }

    public void OpenMenu()
    {
        Show();
        if (GamePlayed)
        {
            _continueButton.Disabled = false;
        }
    }
    public void ContinueGame()
    {
        _continueButton.Disabled = true;
        Hide();
    }

    public void NewGame()
    {
        EmitSignal(SignalName.NewGameSignal);
        GamePlayed = true;
        Hide();
    }

    public void QuitGame()
    {
        GetTree().Quit();
    }
}
