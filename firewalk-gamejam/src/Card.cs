using Godot;
using System;

public partial class Card : Control
{
    // Card Resources
    [Export]
    private CardResource _cardResource;
    private Label _cardNameLabel;
    private Label _cardDescriptionLabel;
    private Label _cardTypeLabel;
    private Label _cardCostLabel;
    private Sprite2D _cardSprite;

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

    // Provide signal when card is dropped
    [Signal]
    public delegate void CardDroppedEventHandler();

    public override void _Ready()
    {
        _cardSprite = GetNode<Sprite2D>("Card Sprite");
        _cardSprite.Texture = _cardResource.CardSrpite;
        
        _cardNameLabel = GetNode<Label>("Card Sprite/Name Label");
        _cardCostLabel = GetNode<Label>("Card Sprite/Cost Label");
        _cardDescriptionLabel = GetNode<Label>("Card Sprite/Description Label");
        _cardTypeLabel = GetNode<Label>("Card Sprite/ Label");

        GenerateCardDescription();

        base._Ready();
    }

    public void SetToDefaultZIndex()
    {
       ZIndex = defaultZIndex;
    }

    private void GenerateCardLabels()
    {
        _cardNameLabel.Text = _cardResource.CardName;
        _cardCostLabel.Text = _cardResource.CardCost.ToString();
        _cardTypeLabel.Text = _cardResource.cardType.ToString();
        GenerateCardDescription();
    }

    private void GenerateCardDescription()
    {
        var Description = "";
        if(_cardResource.DamageValue !=0)
        {
            Description += " Deal {_cardResource.DamageValue} damage \n";
        }
        _cardTypeLabel.Text = Description;
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
                
            }
            else
            {
                isDragging = false;
                if(MouseBrain.current_card_held == this)
                {
                    MouseBrain.current_card_held = null;
                }
                EmitSignal(SignalName.CardDropped);
            }
            return;
        }


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
