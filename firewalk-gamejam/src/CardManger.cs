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

    public override void _Ready()
    {
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
            newCard.Name = "Scratch Card";
            cardsPlayerOwns.Add(newCard);
        }
        for (int i = 0; i < 2; i++)
        {
            Card newCard = (Card)cardBase.Instantiate();
            newCard.cardResource = cardsResourcesList[1];
            newCard.Name = "Bite Card";
            cardsPlayerOwns.Add(newCard);
        }
        for (int i = 0; i < 5; i++)
        {
            Card newCard = (Card)cardBase.Instantiate();
            newCard.cardResource = cardsResourcesList[2];
            newCard.Name = "Steak Card";
            cardsPlayerOwns.Add(newCard);
        }
        for (int i = 0; i < 1; i++)
        {
            Card newCard = (Card)cardBase.Instantiate();
            newCard.cardResource = cardsResourcesList[3];
            newCard.Name = "Howl Card";
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
        cardsInDiscard.Add(card);
    }

    public void AddCardToDeck(Card card)
    {
        cardsInDeck.Add(card);
    }

    public void AddCardToHand()
    {
        if (cardsInDeck.Count > 0)
        {
            playerHand.AddCardToHand(cardsInDeck[0]);
            cardsInDeck.RemoveAt(0);
        }
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


