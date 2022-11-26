using Godot;
using System;

public class Projactile : KinematicBody2D{
	private PackedScene smoke = (PackedScene)ResourceLoader.Load("res://Prefab/Effects/Smoke.tscn");
	public Vector2 velocity = new Vector2();
	private float time;
	public int speed;

	public virtual void Start(Vector2 pos, float dir , Vector2 target = default){
		Rotation = dir;
		Position = pos;
	}

	public override void _PhysicsProcess(float delta){
		var collision = MoveAndCollide(velocity * delta);
		if (collision != null){
			var coll = collision.Collider;
			if (coll.HasMethod("Hit")) coll.Call("Hit", GlobalPosition, 1, this);
		}
  	}

	public void Hit(){
		Node2D _smoke = (Node2D)smoke.Instance();
		_smoke.Position = Position;
		_smoke.Rotation = Rotation;
		GetTree().CurrentScene.AddChild(_smoke);
		QueueFree();
	}
}
