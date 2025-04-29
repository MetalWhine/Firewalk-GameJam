using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHand : Node2D
{
    private List<Card> _playerHand = new List<Card>();
    private float _screenCentreX = DisplayServer.WindowGetSize().X / 2;
    private float _handPositionY = DisplayServer.WindowGetSize().Y * 0.6f;
    const float _cardWidth = 130f;
    const float _animationSpeed = 0.2f;
    private CardManger _cardManager;

    public override void _Ready()
    {
        _cardManager = GetParent<CardManger>();
        base._Ready();
    }

    public void _on_card_card_dropped(bool valid, Card card)
    {
        if (valid && _playerHand.Contains(card))
        {
            RemoveCardFromHand(card);
            _cardManager.AddCardToDiscard(card);
            UpdatePositions();
        }
        else
        {
            UpdatePositions();
        }
    }

    public void DiscardAllCards()
    {
        foreach (Card card in _playerHand)
        {
            _cardManager.AddCardToDiscard(card);
            RemoveCardFromHand(card);
        }
    }

    public void AddCardToHand(Card card)
    {
        _playerHand.Add(card);
        card.CardDropped += _on_card_card_dropped;
        UpdatePositions();
        AddChild(card);
    }

    public void RemoveCardFromHand (Card card)
    {
        if (_playerHand.Contains(card))
        {
            _playerHand.Remove(card);
            UpdatePositions();
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
        var x_offset = (_playerHand.Count * _cardWidth);

        var x_position = _screenCentreX + (index * _cardWidth) - (x_offset/2);

        return x_position;
    }
}
