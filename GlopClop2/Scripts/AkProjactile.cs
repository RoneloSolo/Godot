using Godot;
using System;

public class AkProjactile : KinematicBody2D{
	private Vector2 _velocity = new Vector2();
	float time;
	PackedScene smoke = (PackedScene)ResourceLoader.Load("res://Prefab/Smoke.tscn");
	
	public void Start(Vector2 pos, float dir, float speed){
		Rotation = dir;
		Position = pos;
		_velocity = new Vector2(speed, 0).Rotated(Rotation);
	}
	
  	public override void _PhysicsProcess(float delta){
		var collision = MoveAndCollide(_velocity * delta);
		if (collision != null){
			if (collision.Collider.HasMethod("Hit")){
				collision.Collider.Call("Hit", 1);
				Node2D _smoke = (Node2D)smoke.Instance();
				_smoke.Position = Position;
				GetTree().CurrentScene.AddChild(_smoke);
				QueueFree();
			}
		}
  	}
}
