using Godot;
using System;

public class RocketProjactile : KinematicBody2D{
	public float speed = 2;
	private float time;
	private Vector2 target;
	private Vector2 velocity;
	private Vector2 center;
	private Vector2 start;
	private float r;
	
	private PackedScene blast = (PackedScene)ResourceLoader.Load("res://Prefab/Blast.tscn");

	public void Start(Vector2 pos, float dir, Vector2 tar){
		Rotation = dir;
		Position = pos;
		target = tar;
	}

	public override void _Ready(){
		target += new Vector2(
			(float)GD.RandRange(-45,45),
			(float)GD.RandRange(-45,45)
		);
		r = GlobalPosition.DistanceTo(target);
		start = GlobalPosition;
		center = (target + start) / 2;
		center += new Vector2(
			(float)GD.RandRange(-125,125),
			(float)GD.RandRange(-125,125)
		);
	}

  	public override void _PhysicsProcess(float delta){
		time += delta;
		var disPos = Arc(start, center, target, time / speed);
		velocity = disPos - GlobalPosition;
		LookAt(disPos);

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

	private static Vector2 Arc(Vector2 from, Vector2 b, Vector2 to, float weight){
		var p0 = from.LinearInterpolate(b, weight);
		var p1 = b.LinearInterpolate(to, weight);
		return p0.LinearInterpolate(p1, weight);
	}
}
