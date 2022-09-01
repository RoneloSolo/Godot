using Godot;
using System;

public class RocketProjactile : KinematicBody2D{
	public int speed = 1;
	private float time;
	private Vector2 target;
	private Vector2 velocity;
	
	private PackedScene blast = (PackedScene)ResourceLoader.Load("res://Prefab/Blast.tscn");

	public void Start(Vector2 pos, float dir, Vector2 tar){
		Rotation = dir;
		Position = pos;
		target = tar;
	}
	
  	public override void _PhysicsProcess(float delta){
		velocity = target - GlobalPosition;
		velocity = velocity.Normalized() * speed;
		var collision = MoveAndCollide(velocity);
		if (collision != null){
			if (collision.Collider.HasMethod("Hit")){
				collision.Collider.Call("Hit");
				var _blast = (Node2D)blast.Instance();
				_blast.Position = Position;
				GetTree().CurrentScene.AddChild(_blast);
				QueueFree();
			}
		}
		else if(GlobalPosition.DistanceTo(target) < 1) {
			var _blast = (Node2D)blast.Instance();
			_blast.Position = Position;
			GetTree().CurrentScene.AddChild(_blast);
			QueueFree();
		}
  	}
}
