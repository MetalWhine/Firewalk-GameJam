using Godot;
using System;

public partial class Card : Control
{
    // Card Resources
    [Export]
    public CardResource cardResource;
    private Label _cardNameLabel;
    private Label _cardDescriptionLabel;
    private Label _cardTypeLabel;
    private Label _cardCostLabel;
    private Sprite2D _cardSprite;
    private AnimationPlayer _animationPlayer;

    // Booleans to prevent multiple cards from being dragged
    bool isMouseOver = false;
    bool isDragging = false;

    // Dragging settings
    const float dragSpeed = 22f;

    // Zoom in settings
    const float zoomMultiplier = 1.5f;
    Vector2 zoomMultiplierVector2 = new Vector2(zoomMultiplier, zoomMultiplier);
    const float zoomSpeed = 20f;

    // Hand setting
    public int defaultZIndex = 1;
    private float _validDropYPosition = DisplayServer.WindowGetSize().Y / 2;
    private bool _isValidDropPosition = false;
    private bool _isValidDrop = false;

    // Provide signal when card is dropped
    [Signal]
    public delegate void CardDroppedEventHandler(bool Valid, Card card);
    [Signal]
    public delegate void CardDroppedValidCheckEventHandler(Card card);

    public override void _Ready()
    {
        _cardSprite = GetNode<Sprite2D>("Card Sprite");
        _cardSprite.Texture = cardResource.CardSrpite;
        
        _cardNameLabel = GetNode<Label>("Card Sprite/Name Label");
        _cardCostLabel = GetNode<Label>("Card Sprite/Cost Label");
        _cardDescriptionLabel = GetNode<Label>("Card Sprite/Description Label");
        _cardTypeLabel = GetNode<Label>("Card Sprite/Type Label");

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _animationPlayer.Stop();

        GenerateCardLabels();

        base._Ready();
    }

    public void SetToDefaultZIndex()
    {
       ZIndex = defaultZIndex;
    }

    public override void _PhysicsProcess(double delta)
    {
        drag_logic();
        zoom_logic();
        base._PhysicsProcess(delta);
    }

    private void drag_logic()
    {
        if ((isMouseOver || isDragging) && (MouseBrain.current_card_held == null || MouseBrain.current_card_held == this))
        {
            if (Input.IsActionPressed("Click"))
            {
                GlobalPosition = CustomLerpVector2(GlobalPosition, GetGlobalMousePosition() - (Size*Scale.X / 2), dragSpeed*(float)GetPhysicsProcessDeltaTime());
                isDragging = true;
                MouseBrain.current_card_held = this;
                if (Position.Y <= _validDropYPosition && _isValidDropPosition == false)
                {
                    _isValidDropPosition = true;
                    _animationPlayer.Play("Card_ValidHover");
                }
                else if (Position.Y > _validDropYPosition && _isValidDropPosition == true)
                {
                    _isValidDropPosition = false;
                    _animationPlayer.Stop();
                }
            }
            else if (MouseBrain.current_card_held == this)
            {
                isDragging = false;
                MouseBrain.current_card_held = null;
                EmitSignal(SignalName.CardDropped, _isValidDropPosition, this);
                _animationPlayer.Stop();
                if (_isValidDropPosition)
                {
                    QueueFree();
                }
            }
            return;
        }


    }

    private void GenerateCardLabels()
    {
        _cardNameLabel.Text = cardResource.CardName;
        _cardCostLabel.Text = cardResource.CardCost.ToString();
        _cardTypeLabel.Text = cardResource.cardType.ToString();
        GenerateCardDescription();
    }

    private void GenerateCardDescription()
    {
        string Description = "";
        if (cardResource.DamageValue != 0)
        {
            Description += $" Deal {cardResource.DamageValue} damage \n";
        }
        if (cardResource.AttackValue != 0)
        {
            Description += $" Change attack by {cardResource.DamageValue} \n";
        }
        if (cardResource.ResistanceValue != 0)
        {
            Description += $" Change resistance by {cardResource.ResistanceValue} \n";
        }
        if (cardResource.RageValue != 0)
        {
            Description += $" Change rage by {cardResource.RageValue} \n";
        }
        _cardDescriptionLabel.Text = Description;
    }

    private void zoom_logic()
    {
        if ((isMouseOver || isDragging) && (MouseBrain.current_card_held == null || MouseBrain.current_card_held == this) && Scale != zoomMultiplierVector2)
        {
            Scale = CustomLerpVector2(Scale, zoomMultiplierVector2, zoomSpeed * (float)GetPhysicsProcessDeltaTime());
            ZIndex = 20;
        }
        else if (!isMouseOver && Scale != Vector2.One)
        {
            Scale = CustomLerpVector2(Scale, Vector2.One, zoomSpeed * (float)GetPhysicsProcessDeltaTime());
            ZIndex = defaultZIndex;
        }
        else
        {
            return;
        }
    }

    public void _on_mouse_entered()
    {
        isMouseOver = true;
    }

    public void _on_mouse_exited()
    {
        isMouseOver = false;
    }

    private float CustomLerp(float from, float to, float weight)
    {
        return from * (1-weight) + to * weight;
    }

    private Vector2 CustomLerpVector2(Vector2 from, Vector2 to, float weight)
    {
        return new Vector2(
            CustomLerp(from.X, to.X, weight),
            CustomLerp(from.Y, to.Y, weight)
        );
    }
}
