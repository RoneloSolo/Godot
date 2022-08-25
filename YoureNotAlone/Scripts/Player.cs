using Godot;
using System;

public class Player : KinematicBody2D
{
    Vector2 velocity;
    int speed = 14;

    public override void _Process(float delta)
    {
        var x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
        var y = Input.GetActionStrength("down") - Input.GetActionStrength("up");

        velocity = new Vector2(x,y).Normalized();
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveAndSlide(velocity * speed * speed);
    }
}
