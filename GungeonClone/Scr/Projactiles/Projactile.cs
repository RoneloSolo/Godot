using Godot;
using System;

public class Projactile : KinematicBody2D{
	[Export] public int speed;
	[Export] public int damage;
	private Vector2 velocity;

	public override void _Ready(){
		velocity = new Vector2(speed, 0).Rotated(Rotation);
	}

	public override void _PhysicsProcess(float delta){
		var collision2D = MoveAndCollide(velocity, false);
		if(collision2D == null) return;
		if(collision2D.Collider.HasMethod("Hit")) {
			collision2D.Collider.Call("Hit", damage, velocity);
			QueueFree();
		}
	}
}
