using Godot;
using System;

public class Enemy : KinematicBody2D{
	public void Kill(){
		GD.Print("KIILL");
		QueueFree();
	}
	
	private void _on_Area2D_body_entered(Node2D body){
		if(body.GetName() == "Bullet") Kill();
	}
}



