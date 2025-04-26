using Godot;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public partial class PlayerHand : Node2D
{
    private List<Card> player_hand = new List<Card>();
    float screen_center_x = DisplayServer.WindowGetSize().X / 2;
    float hand_position_y = DisplayServer.WindowGetSize().Y / 4;
    const float card_width = 200f;

    public void AddCardToHand(Card card)
    {
        player_hand.Add(card);
    }

    public void UpdatePositions()
    {
        for (int i = 0; i <= player_hand.Count; i++) {
            player_hand[i].defaultZIndex = i + 1;
            
            var x = CalculateCardPosition(i);
            var new_position = new Vector2(x, hand_position_y);
            var card = player_hand[i];
            animateCardToPosition(card, new_position);
        }
    }

    private void animateCardToPosition(Card card, Vector2 new_position)
    {

    }

    public float CalculateCardPosition(int index)
    {
        var x_offset = (player_hand.Count - 1) * card_width;

        var x_position = screen_center_x + (index * card_width) - (x_offset / 2);

        return x_position;
    }
}
