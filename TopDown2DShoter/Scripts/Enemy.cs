using Godot;
using System;

public class Enemy : KinematicBody2D{
	int chp = 5;
	int mhp = 5;
	
	void Hit(){
		GD.Print("hit");
		chp -= 1;
		if(chp <= 0) Kill();
	}
	
	void Kill(){
		QueueFree();
	}
	
	private void _on_Area2D_body_entered(Node2D body){
		if(body.GetName() == "Bullet") Hit();
	}
}



