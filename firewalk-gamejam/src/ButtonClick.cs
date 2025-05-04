using Godot;
using System;

public partial class ButtonClick : AudioStreamPlayer
{
    public void ButtonClickedResponder()
    {
        this.Play();
    }
}
