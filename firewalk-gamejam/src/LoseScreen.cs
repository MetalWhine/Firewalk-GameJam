using Godot;
using System;

public partial class LoseScreen : Control
{
	private Button _genericButton;

	[Signal]
	public delegate void ButtonClickedEventHandler();

	public override void _Ready()
	{
		_genericButton = GetNode<Button>("Button");
		_genericButton.ButtonDown += ButtonIsClicked;
		Hide();
	}

	public void ShowLoseScreen()
    {
        Show();
    }

	public void ButtonIsClicked()
	{
		EmitSignal(SignalName.ButtonClicked);
	}
}
