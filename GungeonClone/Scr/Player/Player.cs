using Godot;
using System;

public class Player : RigidBody2D{
	private float deltaTime;
	private float time;
	private float speed = 120;
	private Position2D hand;
	private Vector2 mousePos;
	private Sprite playerSpirte;
	private float frictionScale = 4; 
	private float gettingSpeedScale = .8f; 
	private Vector2 topVelocity;
	private int essenceAmount;
	public override void _Ready(){
		hand = GetNode<Position2D>("Hand");
		playerSpirte = GetNode<Sprite>("Sprite");
	}

	public override void _Process(float delta) {
		deltaTime = delta;
		time += deltaTime;
		mousePos = GetGlobalMousePosition();
		HandHendler();
	}

	public override void _PhysicsProcess(float delta) {
		Movement();
	}

	private void Movement() {
		topVelocity.x = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		topVelocity.y = Input.GetActionStrength("down") - Input.GetActionStrength("up");
		topVelocity = topVelocity.Normalized() * speed;
		var desVelocity = topVelocity - LinearVelocity;
		desVelocity = (topVelocity.Length() <= 0) ? desVelocity * frictionScale : desVelocity * gettingSpeedScale;
		AppliedForce = desVelocity;
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state){
		Movement();
	}

	private void HandHendler() {
		if (Position.x < mousePos.x && playerSpirte.FlipH) playerSpirte.FlipH = false;
		else if (Position.x > mousePos.x && !playerSpirte.FlipH) playerSpirte.FlipH = true;
		hand.LookAt(mousePos);
		if (Mathf.Rad2Deg(hand.Rotation) > 180) hand.Rotation = Mathf.Deg2Rad(-181);
		else if (Mathf.Rad2Deg(hand.Rotation) < -180) hand.Rotation = Mathf.Deg2Rad(181);
		if (Mathf.Rad2Deg(hand.Rotation) > -90 && Mathf.Rad2Deg(hand.Rotation) < 90) hand.Scale = new Vector2(1, 1);
		else hand.Scale = new Vector2(1, -1);
	}

	private void CollectEssence(){
		essenceAmount += (int)GD.RandRange(1,5);
	}

	private void OnAreaEnter(Node body){
		CollectEssence();
		body.QueueFree();
	}
}
