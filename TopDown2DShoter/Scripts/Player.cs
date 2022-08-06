using Godot;
using System;

public class Player : KinematicBody2D{

	// player
	private Vector2 vel = new Vector2();
	private float speed = 3.5f;
	Node2D hand;

	int currentWeapon;
	Range range;
	Melle melle;

	float time;
	
	public override void _Ready(){
		hand = GetNode<Node2D>("Hand");
		range = GetNode<Range>("Hand/range");
		melle = GetNode<Melle>("Hand/melle");
		GD.Randomize();
	}
	
	public override void _Process(float delta){
		time += delta;
		range.timeNow = time;
		HandHendler();
		Movement();
		
		if(Input.IsActionPressed("fire")) {
			switch (currentWeapon){
				case 0: range.Fire(); break;	
				case 1: melle.Slash(); break;
			}
		}
	}
	
	public override void _Input(InputEvent inputEvent){	
	}
	
	void Movement(){
		vel.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		vel.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		MoveAndCollide(vel.Normalized() * speed);
	}

	void HandHendler(){
		hand.LookAt(GetGlobalMousePosition());
		if(Rad2Deg(hand.Rotation) > 180) hand.Rotation = Deg2Rad(-181);
		else if(Rad2Deg(hand.Rotation) < -180) hand.Rotation = Deg2Rad(181);
		if(Rad2Deg(hand.Rotation) > -90 && Rad2Deg(hand.Rotation) < 90) hand.SetScale(new Vector2(1,1));
		else hand.SetScale(new Vector2(1,-1));
	}

	float Rad2Deg(float num){
		return num * 180 / Mathf.Pi;
	}
	
	float Deg2Rad(float num){
		return num * Mathf.Pi / 180;
	}
	
}
