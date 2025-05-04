using Godot;
using System;
using System.Collections.Generic;

public partial class CardSelectScreen : Control
{

	private Card _card1;
    private Card _card2;
	private Card _card3;

	private Button _buttonCard1;
	private Button _buttonCard2;
	private Button _buttonCard3;

	private Card[] _shownCards = new Card[3];

	private Button _skipCard;

	[Signal]
	public delegate void CardSelectedEventHandler(Card card = null);
	[Signal]
	public delegate void CardSkippedEventHandler();

	public override void _Ready()
	{
		GetAllNodes();
		Hide();
	}

	public void GetAllNodes()
	{
		_card1 = GetNode<Card>("Card 1");
		_card2 = GetNode<Card>("Card 2");
		_card3 = GetNode<Card>("Card 3");

		_buttonCard1 = GetNode<Button>("Card 1/Card button 1");
		_buttonCard2 = GetNode<Button>("Card 2/Card button 2");
		_buttonCard3 = GetNode<Button>("Card 3/Card button 3");

		_skipCard = GetNode<Button>("Skip Card Button");
		
		_shownCards[0] = _card1;
		_shownCards[1] = _card2;
		_shownCards[2] = _card3;
	}

	public void CardClicked(int index)
	{
		switch (index)
		{
			case 0:
			case 1:
			case 2:
                EmitSignal(SignalName.CardSelected, _shownCards[index]);
                break;
			default:
                EmitSignal(SignalName.CardSkipped);
                break;
		}
		Hide();
	}

	public void ShowCardSelectScreen(Card cardSelection1, Card cardSelection2, Card cardSelection3)
	{
		_shownCards[0].cardResource = cardSelection1.cardResource;
		_shownCards[0].Name = cardSelection1.Name;
		_shownCards[0].InitializeCard();
		_shownCards[1].cardResource = cardSelection2.cardResource;
		_shownCards[1].Name = cardSelection2.Name;
		_shownCards[1].InitializeCard();		
		_shownCards[2].cardResource = cardSelection3.cardResource;
		_shownCards[2].Name = cardSelection3.Name;
		_shownCards[2].InitializeCard();
		Show();
	}


}
