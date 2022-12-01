using Godot;
using System;

public class Enemy : RigidBody2D{

	public int health = 1;
	public int speed = 25;
	private Vector2 vel;
	public Vector2 center;
	public Vector2 extraVel;
	public Node2D player;

	private Vector2[] dir = new Vector2[8]{Vector2.Right , new Vector2(1,-1), Vector2.Up, new Vector2(-1,-1), Vector2.Left, new Vector2(-1,1), Vector2.Down, -new Vector2(1,1)};
	private RayCast2D[] collRay = new RayCast2D[8];
	private float[] interest_dir = new float[8];

	private PackedScene essencePrefab = GD.Load<PackedScene>("res://Prefab/Essence.tscn");
	private float[] deinterest_dir = new float[8];

	public override void _Ready(){
		for(int i = 0; i < 8; i++){
			collRay[i] = GetNode<RayCast2D>("" + i);
		}
		player = GetTree().CurrentScene.GetNode<Node2D>("YSort/Player");
	}

	public override void _IntegrateForces(Physics2DDirectBodyState state){
		WalkAwayPlayer();
	}

	private Vector2 CalculateDiraction(Vector2 tar){
		var to_tar = (tar - GlobalPosition).Normalized();
		float y;

		for(int i = 0; i < 8; i++){
			y = to_tar.Dot(dir[i]);
			interest_dir[i] = y;
		}
		float x = 0;
		y = 0;
		int z = 0;
		for(int i = 0; i < 8; i++){
			if(!collRay[i].IsColliding())  deinterest_dir[i] = 0;
			else {
				if(i > 0) deinterest_dir[i-1] = .5f; else deinterest_dir[7] = .5f;
				deinterest_dir[i] = 5;
				if(i < 7) deinterest_dir[i+1] = .5f; else deinterest_dir[0] = .5f;
			}

			y = interest_dir[i] - deinterest_dir[i];
			if(y > x) {
				x = y;
				z = i;
			}
		}
		return dir[z].Normalized();
	}
	
	public void WalkToPlayer(){
		vel = CalculateDiraction(player.GlobalPosition) * speed;
		vel -=LinearVelocity;
		AppliedForce = vel;
	}
	
	public void WalkAwayPlayer(){
		vel = CalculateDiraction(-player.GlobalPosition) * speed;
		vel -=LinearVelocity;
		AppliedForce = vel;
	}

	public void Hit(int i, Vector2 vel){
		health -=i;
		if(health<=0){
			Die();
			return;
		}
		ApplyImpulse(center, vel * vel * vel);
	}

	private void Die(){
		var amonutToDrop = (int)GD.RandRange(1,5);
		for (int i = 0; i < amonutToDrop; i++){
			var essance = (Node2D)essencePrefab.Instance();
			essance.GlobalPosition = GlobalPosition + new Vector2((float)GD.RandRange(-10,10),(float)GD.RandRange(-10,10));
			GetTree().CurrentScene.AddChild(essance);
		}
		QueueFree();
	}
}
