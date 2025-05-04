using Godot;
using System;

public partial class TutorialAndStoryScreen : Control
{
    [Export]
    private Panel[] _panels;

    private int _currentPanelIndex = 0;

    [Signal]
    public delegate void StartGameEventHandler();

    public override void _Ready()
    {
        Hide();
        base._Ready();
    }

    public void ShowTutorialScreen()
    {
        Show();
        _currentPanelIndex = 0;
        foreach (var panel in _panels)
        {
            panel.Position = new Vector2(0, 1000);
        }
        _panels[_currentPanelIndex].Position = new Vector2(0, 0);
    }

    public void NextScreen()
    {
        int i = _currentPanelIndex + 1;
        if (i < _panels.Length)
        {
            _panels[_currentPanelIndex].Position = new Vector2(0, 1000);
            _currentPanelIndex++;
            _panels[_currentPanelIndex].Position = new Vector2(0, 0);
        }
        else
        {
            EmitSignal(SignalName.StartGame);
            Hide();
        }
    }

    public void PreviousScreen()
    {
        int i = _currentPanelIndex - 1;
        if (i >= 0)
        {
            _panels[_currentPanelIndex].Position = new Vector2(0, 1000);
            _currentPanelIndex--;
            _panels[_currentPanelIndex].Position = new Vector2(0, 0);
        }
    }
}
