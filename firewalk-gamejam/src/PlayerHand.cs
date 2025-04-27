using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public partial class PlayerHand : Node2D
{
    private List<Card> _playerHand = new List<Card>();
    private float _screenCentreX = DisplayServer.WindowGetSize().X / 2;
    private float _handPositionY = DisplayServer.WindowGetSize().Y * 0.75f;
    const float _cardWidth = 130f;
    const float _animationSpeed = 0.2f;

    // Testing variables
    private int _cardCount = 3;
    [Export]
    private PackedScene _cardScene { get; set; }


    // Debugging
    public override void _Ready()
    {
        for (int i = 0; i < _cardCount; i++) {
            var NewCard = _cardScene.Instantiate<Card>();
            AddCardToHand(NewCard);
            NewCard.CardDropped += _on_card_card_dropped;
            AddChild(NewCard);
        }
        base._Ready();
    }

    // Debugging
    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            var NewCard = _cardScene.Instantiate<Card>();
            AddCardToHand(NewCard);
            AddChild(NewCard);
        }


        base._PhysicsProcess(delta);
    }

    public void _on_card_card_dropped()
    {
        UpdatePositions();
    }

    public void AddCardToHand(Card card)
    {
        _playerHand.Add(card);
        UpdatePositions();
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
