using Godot;
using System;

public class AkBullet : KinematicBody2D{
	public int speed = 750;
	private Vector2 _velocity = new Vector2();
	float time;
	
	public void Start(Vector2 pos, float dir){
		Rotation = dir;
		Position = pos;
		_velocity = new Vector2(speed, 0).Rotated(Rotation);
	}
	
  	public override void _PhysicsProcess(float delta){
		var collision = MoveAndCollide(_velocity * delta);
		if (collision != null){
			if (collision.Collider.HasMethod("Hit")){
				QueueFree();
				collision.Collider.Call("Hit");
			}
		}
  	}
}
