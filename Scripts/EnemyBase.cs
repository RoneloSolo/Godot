using Godot;
using System;

public class EnemyBase : KinematicBody{
	private int health = 200;

	public void Hit(int i){
		health -= i;
		if(health <= 0) QueueFree();
	}
}
