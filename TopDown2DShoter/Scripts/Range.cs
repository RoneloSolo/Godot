using Godot;
using System;

public class Range : Node2D{
	Node2D hand;
	Node2D muzzlePoint;
	float bulletSpeed = 2000;
	PackedScene bullet = (PackedScene)ResourceLoader.Load("res://Scene/Bullet.tscn");
	float timeLastShot,shotDelay = .5f;
	public float timeNow;

	public override void _Ready(){
		hand = GetNode<Node2D>("../");
		muzzlePoint = GetNode<Node2D>("ShotPoint");
	}

	public void Fire(){
		if(timeLastShot + shotDelay >= timeNow) return;
		timeLastShot = timeNow;
		var bull = (Bullet)bullet.Instance();
		bull.Start(muzzlePoint.GlobalPosition, hand.Rotation + (float)GD.RandRange(-0.125f,0.125f));
		GetTree().CurrentScene.AddChild(bull);
	}
}
