using Godot;
using System;

public partial class Card : Control
{
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
