using Godot;
using System;

public partial class Card : Control
{
    #region CARD CHILD NODES
    private Label _cardNameLabel;
    private Label _cardDescriptionLabel;
    private Label _cardTypeLabel;
    private Label _cardCostLabel;
    private Sprite2D _cardSprite;
    private AnimationPlayer _animationPlayer;
    #endregion

    #region BOOLEAN CHECKS
    private bool _isMouseOver = false;
    private bool _isDragging = false;
    private bool _isValidDropPosition = false;
    private bool _isValidDrop = false;
    #endregion

    #region CARD SETTINGS
    // Editable Card settings
    [Export]
    private bool forSelection = false;
    [Export]
    private float _dragSpeed = 22f;
    [Export]
    private float _zoomMultiplier = 1.5f;
    [Export]
    private float zoomSpeed = 20f;

    // Non Editable Card settings
    Vector2 _zoomMultiplierVector2;
    public int defaultZIndex = 1;
    private float _validDropYPosition = DisplayServer.WindowGetSize().Y / 2;

    // Card template
    public CardResource cardResource;
    #endregion

    #region SIGNALS AND DELEGATES
    [Signal]
    public delegate void CardDroppedEventHandler(bool Valid, Card card);
    [Signal]
    public delegate void CardDroppedValidCheckEventHandler(Card card);
    #endregion

    public override void _Ready()
    {
        if (!forSelection) { InitializeCard(); }
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!forSelection)
        {
            DragLogic();
            ZoomLogic();
        }
        base._PhysicsProcess(delta);
    }

    public void ValidityCheckReceiver(bool validity)
    {
        if (validity)
        {
            _animationPlayer.Stop();
            EmitSignal(SignalName.CardDropped, validity, this);
            QueueFree();
        }
        else
        {
            EmitSignal(SignalName.CardDropped, validity, this);
            _animationPlayer.Stop();
        }
    }

    private void DragLogic()
    {
        if ((_isMouseOver || _isDragging) && (MouseBrain.current_card_held == null || MouseBrain.current_card_held == this))
        {
            if (Input.IsActionPressed("Click"))
            {
                GlobalPosition = CustomLerpVector2(GlobalPosition, GetGlobalMousePosition() - (Size*Scale.X / 2), _dragSpeed*(float)GetPhysicsProcessDeltaTime());
                _isDragging = true;
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
                _isDragging = false;
                MouseBrain.current_card_held = null;
                if (_isValidDropPosition)
                {
                    ValidityCheck();
                }
                else
                {
                    EmitSignal(SignalName.CardDropped, false, this);
                }
            }
            return;
        }


    }

    private void ZoomLogic()
    {
        if ((_isMouseOver || _isDragging) && (MouseBrain.current_card_held == null || MouseBrain.current_card_held == this) && Scale != _zoomMultiplierVector2)
        {
            Scale = CustomLerpVector2(Scale, _zoomMultiplierVector2, zoomSpeed * (float)GetPhysicsProcessDeltaTime());
            ZIndex = 20;
        }
        else if (!_isMouseOver && Scale != Vector2.One)
        {
            Scale = CustomLerpVector2(Scale, Vector2.One, zoomSpeed * (float)GetPhysicsProcessDeltaTime());
            ZIndex = defaultZIndex;
        }
        else
        {
            return;
        }
    }

    #region INITIALIZE FUNCTIONS

    public void InitializeCard()
    {
        _zoomMultiplierVector2 = new Vector2(_zoomMultiplier, _zoomMultiplier);
        _cardSprite = GetNode<Sprite2D>("Card Sprite");
        _cardSprite.Texture = cardResource.CardSrpite;

        _cardNameLabel = GetNode<Label>("Card Sprite/Name Label");
        _cardCostLabel = GetNode<Label>("Card Sprite/Cost Label");
        _cardDescriptionLabel = GetNode<Label>("Card Sprite/Description Label");
        _cardTypeLabel = GetNode<Label>("Card Sprite/Type Label");

        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        _animationPlayer.Stop();

        GenerateCardLabels();
    }

    public void GenerateCardLabels(int damageModifier = 0, int attackModifier = 0, int resistanceModifier = 0, int rageModifier = 0)
    {
        _cardNameLabel.Text = cardResource.CardName;
        _cardCostLabel.Text = cardResource.CardCost.ToString();
        _cardTypeLabel.Text = cardResource.cardType.ToString();
        GenerateCardDescription(damageModifier, attackModifier, resistanceModifier, rageModifier);
    }

    public void GenerateCardDescription(int damageModifier = 0, int attackModifier = 0, int resistanceModifier = 0, int rageModifier = 0)
    {
        string Description = "";
        if (cardResource.DamageValue != 0)
        {
            Description += $" Deal {cardResource.DamageValue + damageModifier} damage \n";
        }
        if (cardResource.AttackValue != 0)
        {
            Description += $" Change attack by {cardResource.AttackValue + attackModifier} \n";
        }
        if (cardResource.ResistanceValue != 0)
        {
            Description += $" Change resistance by {cardResource.ResistanceValue + resistanceModifier} \n";
        }
        if (cardResource.RageValue != 0)
        {
            Description += $" Change rage by {cardResource.RageValue + rageModifier} \n";
        }
        _cardDescriptionLabel.Text = Description;
    }

    #endregion

    #region TINY FUNCTIONS
    // Signal receivers
    public void _on_mouse_entered()
    {
        _isMouseOver = true;
    }

    public void _on_mouse_exited()
    {
        _isMouseOver = false;
    }

    // Getter Setters
    public void SetToDefaultZIndex()
    {
        ZIndex = defaultZIndex;
    }

    // Validity Check
    private void ValidityCheck()
    {
        EmitSignal(SignalName.CardDroppedValidCheck, this);
    }
    #endregion

    #region CUSTOM LERP IMPLEMENTATION
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
    #endregion
}
