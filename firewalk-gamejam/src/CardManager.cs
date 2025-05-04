using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CardManager : Node
{
    public int currentEnergy { get; set; }
    public int maxEnergy { get; set; }

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
    [Signal]
    public delegate void CardPlayedEventHandler(Card card);

    public override void _Ready()
    {
        _deckCountLabel = GetNode<Label>("Deck View/Deck Count Label");
        _energyLabel = GetNode<Label>("Energy View/Energy Label");
        _discardLabel = GetNode<Label>("Discard View/Discard Count Label");
        playerHand = GetNode<PlayerHand>("PlayerHand");
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
       playerHand.DiscardAllCards();
       cardsInDeck.Clear();
       cardsInDiscard.Clear();


        foreach (Card card in cardsPlayerOwns)
        {
            Card newcard = (Card)cardBase.Instantiate();
            newcard.cardResource = card.cardResource;
            newcard.Name = card.Name;
            cardsInDeck.Add(newcard);
        }


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

    public void PlayCard(Card card)
    {
        EmitSignal(SignalName.CardPlayed, card);
    }

    public void AddCardToDeck(Card card)
    {
        cardsInDeck.Add(card);
        UpdateLabels();
    }

    public void UpdateLabels()
    {
        _deckCountLabel.Text = cardsInDeck.Count.ToString();
        _discardLabel.Text = cardsInDiscard.Count.ToString();
        _energyLabel.Text = currentEnergy.ToString() + "/" + maxEnergy.ToString();
    }

    public void AddCardToHand(int count)
    {
        while (count > 0)
        {
            count--;
            if (cardsInDeck.Count > 0)
            {
                playerHand.AddCardToHand(cardsInDeck[0]);
                cardsInDeck.RemoveAt(0);
            }
            else
            {
                if (cardsInDiscard.Count > 0)
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
    }

    public void CheckValidity(Card card)
    {
        int energyCost = card.cardResource.CardCost;
        if (currentEnergy >= energyCost)
        {
            currentEnergy -= energyCost;
            UpdateLabels();
            card.ValidityCheckReceiver(true);
        }
        else
        {
            card.ValidityCheckReceiver(false);
        }
    }

    public void ResetEnergy()
    {
        currentEnergy = maxEnergy;
        UpdateLabels();
    }
}


