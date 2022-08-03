using Godot;
using System;

public class Bullet : RigidBody2D{
	float BulletSpeed = 500f;
	float time;
	
  	public override void _Process(float delta){
		time += delta;
		if(time > 5f) QueueFree();
		GlobalTranslate(Transform.x * BulletSpeed * delta);
  	}
}
