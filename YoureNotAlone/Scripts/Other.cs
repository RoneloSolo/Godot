using Godot;
using System;

public class Other : KinematicBody2D
{
    Node2D player;
    bool follow;

    public override void _Ready()
    {
        player = GetNode<Node2D>("../You");
    }

    public override void _PhysicsProcess(float delta)
    {
        if(!follow) return;
        var velocity = player.GlobalPosition - GlobalPosition;
        if(velocity.Length() < 100) return;
        MoveAndSlide(velocity.Normalized() * 150);
    }

    private void OnAreaEnter(Node2D node)
    {
        if(follow) return;
        if(node.Name == "You")
        {
            follow = true;
        }
    }
}
