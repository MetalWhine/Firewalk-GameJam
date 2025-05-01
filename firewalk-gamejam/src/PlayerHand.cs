using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHand : Node2D
{
    private List<Card> _playerHand = new List<Card>();
    private float _screenCentreX = DisplayServer.WindowGetSize().X / 2;
    private float _handPositionY = DisplayServer.WindowGetSize().Y * 0.62f;
    const float _cardWidth = 130f;
    const float _animationSpeed = 0.2f;
    private CardManager _cardManager;

    public override void _Ready()
    {
        _cardManager = GetParent<CardManager>();
        base._Ready();
    }

    public void CardDroppedValidityCheck(Card card)
    {
        _cardManager.CheckValidity(card);
    }

    public void OnCardDropped(bool valid, Card card)
    {
        if (valid && _playerHand.Contains(card))
        {
            PlayCardFromHand(card);
        }
        UpdatePositions();
    }

    public void DiscardAllCards()
    {
        foreach (Card card in _playerHand)
        {
            _cardManager.AddCardToDiscard(card);
            card.QueueFree();
        }
        _playerHand.Clear();
    }

    public void AddCardToHand(Card card)
    {
        _playerHand.Add(card);
        card.CardDropped += OnCardDropped;
        card.CardDroppedValidCheck += CardDroppedValidityCheck;
        UpdatePositions();
        AddChild(card);
    }

    public void PlayCardFromHand (Card card)
    {
        if (_playerHand.Contains(card))
        {
            _cardManager.PlayCard(card);
            _cardManager.AddCardToDiscard(card);
            _playerHand.Remove(card);
        }
    }

    public void UpdatePositions()
    {
        for (int i = 0; i < _playerHand.Count; i++) {
            _playerHand[i].defaultZIndex = _playerHand.Count-i;
            _playerHand[i].SetToDefaultZIndex();
            
            var x = CalculateCardPosition(i);
            var new_position = new Vector2(x, _handPositionY);
            var card = _playerHand[i];
            AnimateCardToPosition(card, new_position);
        }
    }

    private void AnimateCardToPosition(Card card, Vector2 new_position)
    {
        card.Position = new_position;
    }

    private float CalculateCardPosition(int index)
    {
        var x_offset = ((_playerHand.Count+0.5f) * _cardWidth);

        var x_position = _screenCentreX + (index * _cardWidth) - (x_offset/2);

        return x_position;
    }
}
