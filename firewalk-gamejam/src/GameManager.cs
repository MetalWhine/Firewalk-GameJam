using Godot;
using System;

public partial class GameManager : Node
{
	public CombatManager combatManager;
	public CardSelectScreen cardSelectScreen;
	public MainMenuScene mainMenuScene;
	public Button mainMenuButton;
	public LoseScreen loseScreen;

	public override void _Ready()
	{
		combatManager = GetNode<CombatManager>("CombatScreen");
		cardSelectScreen = GetNode<CardSelectScreen>("Card Select Scene");
		mainMenuScene = GetNode<MainMenuScene>("Main Menu Scene");
		mainMenuButton = GetNode<Button>("Main Menu Button");
		loseScreen = GetNode<LoseScreen>("Lose Screen");

		combatManager.CardSelectCall += cardSelectScreen.ShowCardSelectScreen;
		cardSelectScreen.CardSelected += combatManager.CardSelectedHandler;
		cardSelectScreen.CardSkipped += combatManager.CardSkippedHandler;
		mainMenuScene.StartGameSignal += combatManager.NewGameStarted;
		mainMenuButton.ButtonDown += mainMenuScene.OpenMenu;

		combatManager._player.GameOver += GameOver;
		loseScreen.ButtonClicked += mainMenuScene.OpenMenu;
    }

	public void GameOver()
	{
		combatManager.ResetGame();
        cardSelectScreen.Hide();
		mainMenuScene.GamePlayed = false;
		loseScreen.ShowLoseScreen();
	}
}
