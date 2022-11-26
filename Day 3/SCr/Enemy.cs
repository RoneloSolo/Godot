using Godot;
using System;

public class Enemy : KinematicBody2D{
	int hp = 2;
	Node2D castle;
	
	public override void _Ready()=> castle = GetTree().CurrentScene.GetNode<Node2D>("Castle");
	
	public override void _PhysicsProcess(float delta){
		LookAt(castle.Position);
		MoveAndCollide(Vector2.Right.Rotated(Rotation).Normalized());
	}
	
	public void Hit(){
		hp --;
		if(hp<=0) QueueFree();
	}
}
