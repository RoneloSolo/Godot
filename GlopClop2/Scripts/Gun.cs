using Godot;
using System;

public class Gun : Node2D
{
    [Export] public float fireRate;
    public float lastTimeFire;
    public PackedScene projactile;
    public Node2D point;

    public override void _Ready()
    {
        point = GetNode<Node2D>("Point");    
    }

    public virtual void Fire(float time){}
    
    public void On()
    {
        Visible = true;
    }
    
    public void Off()
    {
        Visible = false;
    }
}
