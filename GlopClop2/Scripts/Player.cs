using Godot;
using System;

public class Player : KinematicBody2D{

	// player
	private Vector2 vel = new Vector2();
	private float speed = 3.5f;
	Node2D hand;
	Sprite sprite;
	Vector2 mousePos;

	Node2D muzzlePoint;
	float bulletSpeed = 2000;
	PackedScene akBullet = (PackedScene)ResourceLoader.Load("res://Scene/AkBullet.tscn");
	float timeLastShot,shotDelay = .5f;
	public float time;

	public override void _Ready(){
		hand = GetNode<Node2D>("Hand");
		sprite = GetNode<Sprite>("Sprite");
		muzzlePoint = GetNode<Node2D>("Hand/Ak/ShotPoint");
		GD.Randomize();
	}
	
	public override void _Process(float delta){
		time += delta;
		mousePos = GetGlobalMousePosition();
		if(Input.IsActionPressed("Fire")) Fire();
		Movement();
		HandHendler();
		BodyHendler();
	}

		public override void _Input(InputEvent inputEvent){
		if(Input.IsActionPressed("Fire")) Fire();
	}
	
	public void Fire(){
		if(timeLastShot + shotDelay >= time) return;
		timeLastShot = time;
		var bull = (AkBullet)akBullet.Instance();
		bull.Start( muzzlePoint.GlobalPosition, hand.Rotation + (float)GD.RandRange(-0.125f,0.125f));
		GetTree().CurrentScene.AddChild(bull);
	}

	void Movement(){
		vel.x = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		vel.y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
		MoveAndCollide(vel.Normalized() * speed);
	}

	void HandHendler(){
		hand.LookAt(mousePos);
		if(Rad2Deg(hand.Rotation) > 180) hand.Rotation = Deg2Rad(-181);
		else if(Rad2Deg(hand.Rotation) < -180) hand.Rotation = Deg2Rad(181);
		if(Rad2Deg(hand.Rotation) > -90 && Rad2Deg(hand.Rotation) < 90) hand.Scale = new Vector2(1,1);
		else hand.Scale = new Vector2(1,-1);
	}
	
	void BodyHendler(){
		if(mousePos.x > Position.x) sprite.FlipH = false;
		else if(mousePos.x < Position.x) sprite.FlipH = true;
	}

	float Rad2Deg(float num){
		return num * 180 / Mathf.Pi;
	}
	
	float Deg2Rad(float num){
		return num * Mathf.Pi / 180;
	}
}
