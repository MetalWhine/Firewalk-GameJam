using Godot;
using System;
using System.Collections.Generic;

public partial class CardManger : Node
{
    [Export]
    public CardResource[] cardsResourcesList;

    public List<Card> cardsPlayerOwns = new List<Card>();
    public List<Card> cardsInDeck = new List<Card>();
    public List<Card> cardsInDiscard = new List<Card>();

    public PlayerHand playerHand;
    [Export]
    public PackedScene cardBase;

    #region UI ELEMENTS
    private Label _deckCountLabel;
    private Label _energyLabel;
    private Label _discardLabel;
    #endregion

    public override void _Ready()
    {
        _deckCountLabel = GetNode<Label>("Deck View/Deck Count Label");
        _energyLabel = GetNode<Label>("Energy View/Energy Label");
        _discardLabel = GetNode<Label>("Discard View/Discard Count Label");
        playerHand = GetNode<PlayerHand>("PlayerHand");
        GenerateStartingDeck();
        base._Ready();
    }

    public void GenerateStartingDeck()
    {
        cardsPlayerOwns.Clear();
        for (int i = 0; i < 5; i++)
        {
            Card newCard = (Card)cardBase.Instantiate();
            newCard.cardResource = cardsResourcesList[0];
            newCard.Name = newCard.cardResource.CardName;
            cardsPlayerOwns.Add(newCard);
        }
        for (int i = 0; i < 2; i++)
        {
            Card newCard = (Card)cardBase.Instantiate();
            newCard.cardResource = cardsResourcesList[1];
            newCard.Name = newCard.cardResource.CardName;
            cardsPlayerOwns.Add(newCard);
        }
        for (int i = 0; i < 5; i++)
        {
            Card newCard = (Card)cardBase.Instantiate();
            newCard.cardResource = cardsResourcesList[2];
            newCard.Name = newCard.cardResource.CardName;
            cardsPlayerOwns.Add(newCard);
        }
        for (int i = 0; i < 1; i++)
        {
            Card newCard = (Card)cardBase.Instantiate();
            newCard.cardResource = cardsResourcesList[3];
            newCard.Name = newCard.cardResource.CardName;
            cardsPlayerOwns.Add(newCard);
        }
        ResetDeck();
    }

    public void ResetDeck()
    {
       cardsInDeck.Clear();
       cardsInDiscard.Clear();
       cardsInDeck = cardsPlayerOwns;
       cardsInDeck = ShuffleCards(cardsInDeck);
       UpdateLabels();
    }

    private List<Card> ShuffleCards(List<Card> initialList)
    {
        Random random = new Random();
        List<Card> shuffledList = new List<Card>(initialList);
        int x = initialList.Count;
        int z;

        for (int i = 0; i < x; i++)
        {
            z = random.Next(initialList.Count);
            shuffledList[i] = initialList[z];
            initialList.RemoveAt(z);
        }

        return shuffledList;
    }

    public void AddCardToDiscard(Card card)
    {
        Card newcard = (Card)cardBase.Instantiate();
        newcard.cardResource = card.cardResource;
        newcard.Name = card.Name;
        cardsInDiscard.Add(newcard);
        UpdateLabels();
    }

    public void AddCardToDeck(Card card)
    {
        cardsInDeck.Add(card);
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        _deckCountLabel.Text = cardsInDeck.Count.ToString();
        _discardLabel.Text = cardsInDiscard.Count.ToString();
    }

    public void AddCardToHand()
    {
        if (cardsInDeck.Count > 0)
        {
            playerHand.AddCardToHand(cardsInDeck[0]);
            cardsInDeck.RemoveAt(0);
        }
        else
        {
            if(cardsInDiscard.Count > 0)
            {
                foreach (Card card in cardsInDiscard)
                {
                    cardsInDeck.Add(card);
                }
                cardsInDeck = ShuffleCards(cardsInDeck);
                cardsInDiscard.Clear();
                playerHand.AddCardToHand(cardsInDeck[0]);
                cardsInDeck.RemoveAt(0);
            }
        }
        UpdateLabels();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            AddCardToHand();
        }
        base._Process(delta);
    }
}


