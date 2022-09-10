using Godot;
using System;

public class Gun : Node2D{
	public GameManager gameManager;

	[Export] public float fireRate;
	public float lastTimeFire;

	public PackedScene projactile;
	public Node2D point;

	public AnimationPlayer anim;

	public override void _Ready(){
		anim = GetNode<AnimationPlayer>("../Anim");    
		gameManager = GetTree().CurrentScene.GetNode<GameManager>("GameManager");
	}

	public virtual void Fire(float time){}

	public void On() => Visible = true;
	
	public void Off() => Visible = false;
}
