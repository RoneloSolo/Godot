using Godot;
using System;

public class Player : KinematicBody2D{

	private Vector2 vel = new Vector2();
	private float speed = 3.5f;
	float bulletSpeed = 2000;
	PackedScene bullet = (PackedScene)ResourceLoader.Load("res://Scene/Bullet.tscn");
	Node2D hand;
	Node2D muzzlePoint;
	float timeLastShot,timeNow,shotDelay = 0f;
	
	public override void _Ready(){
		hand = GetNode<Node2D>("Hand");
		muzzlePoint = GetNode<Node2D>("Hand/ShotPoint");
		GD.Randomize();
	}
	
	public override void _Process(float delta){
		timeNow += delta;
		vel.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		vel.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		MoveAndCollide(vel.Normalized() * speed);
		hand.LookAt(GetGlobalMousePosition());

		if(Rad2Deg(hand.Rotation) > 180) hand.Rotation = Deg2Rad(-181);
		else if(Rad2Deg(hand.Rotation) < -180) hand.Rotation = Deg2Rad(181);
		
		if(Rad2Deg(hand.Rotation) > -90 && Rad2Deg(hand.Rotation) < 90) hand.SetScale(new Vector2(1,1));
		else hand.SetScale(new Vector2(1,-1));
		
		if(Input.IsActionPressed("fire")) {
			if(timeLastShot + shotDelay < timeNow){
				timeLastShot = timeNow;
				Fire();
			}
		}
	}
	
	public override void _Input(InputEvent inputEvent){	
		if (inputEvent.IsActionPressed("fire")) Fire();
	}
	
	void Fire(){
		Node2D bull = (Node2D)bullet.Instance();
		bull.Rotation = hand.Rotation + (float)GD.RandRange(-0.125f,0.125f);
		bull.Position = muzzlePoint.GlobalPosition;
		GetTree().CurrentScene.AddChild(bull);
	}
	
	float Rad2Deg(float num){
		return num * 180 / Mathf.Pi;
	}
	
	float Deg2Rad(float num){
		return num * Mathf.Pi / 180;
	}
}
