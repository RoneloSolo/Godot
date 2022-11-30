using Godot;
using System;

public class Player : RigidBody2D{
	
	//  Movement
	private int facing;
	private Vector2 velocity;
	private Area2D groundCheckArea;
	private bool isOnFloor;
	private bool isAlredyJumped;
	private float inputVelocity;
	[Export] private float speed = 250;
	[Export] private float startScale = 3f;
	[Export] private float frictionScale = .2f;
	[Export] private float thrust = 25;
	
	//  Fire
	private float time;
	private float lastTimeShoot;
	[Export] private float shootDelay;
	private PackedScene bulletPrefab = (PackedScene)ResourceLoader.Load("res://Prefab/Bullet.tscn");
	private Position2D[] shootPoint = new Position2D[2];


	public override void _Ready(){
		shootPoint[0] = GetNode<Position2D>("ShootPointR");
		shootPoint[1] = GetNode<Position2D>("ShootPointL");
		groundCheckArea = GetNode<Area2D>("GroundCheck");
		groundCheckArea.Connect("body_entered", this, "groundCheckEnter");
		groundCheckArea.Connect("body_exited", this, "groundCheckExit");
	}

	private void groundCheckEnter(object body){
		isOnFloor = true;
		isAlredyJumped = false;
	}
	private void groundCheckExit(object body) => isOnFloor = false;

	private void Movement() {
		inputVelocity = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		velocity.x = inputVelocity * speed;
		velocity.x -= LinearVelocity.x;
		velocity.x = (inputVelocity == 0) ? velocity.x * frictionScale : velocity.x * startScale;
		AppliedForce = velocity;
		if(Input.IsActionJustPressed("jump")){
			if(!isOnFloor || isAlredyJumped) return;
			isAlredyJumped = true;
			AddForce(Vector2.Zero, Vector2.Up * thrust * thrust);
		}
	}

	private void Fire(float delta){
		time += delta;
		if(time >= lastTimeShoot+shootDelay){
			lastTimeShoot = time;
			var bullet = (Bullet)bulletPrefab.Instance();
			bullet.Spawn(shootPoint[facing]);
			GetTree().CurrentScene.AddChild(bullet);
		}
	}
	

	public override void _IntegrateForces(Physics2DDirectBodyState state){
		Movement();
	}

	public override void _Process(float delta){
		if(Input.IsActionPressed("fireL")) Fire(delta);
		if(inputVelocity>0) facing = 0;
		else if(inputVelocity<0) facing = 1;
	}
	
}
