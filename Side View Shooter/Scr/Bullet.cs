using Godot;
using System;

public class Bullet : KinematicBody2D{
	private const int SPEED = 750;
	private Vector2 velocity;
	private float time;
	
	public void Spawn(Position2D point){
		Position = point.GlobalPosition;
		velocity = new Vector2(SPEED, 0).Rotated(point.Rotation);
	}
	
  	public override void _PhysicsProcess(float delta){
		var collision = MoveAndCollide(velocity * delta);
		if (collision != null){
			if (collision.Collider.HasMethod("Hit")){
				QueueFree();
				collision.Collider.Call("Hit");
			}
		}
  	}
}
