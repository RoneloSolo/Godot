using Godot;
using System;

public class EnemyBase : KinematicBody2D{
	PackedScene smoke = (PackedScene)ResourceLoader.Load("res://Prefab/Smoke.tscn");
	Node2D player;
	[Export] public int health = 3;
	AnimationPlayer anim;

	public override void _Ready(){
		anim = GetNode<AnimationPlayer>("Anim");
		player = GetTree().CurrentScene.GetNode<Node2D>("GamePlay/Player");
		anim.Play("Walk");
	}

	public override void _PhysicsProcess(float delta){
		if(anim.CurrentAnimation == "" && anim.CurrentAnimationPosition >= anim.CurrentAnimationLength) anim.Play("Walk");
		var vel = player.Position - Position;
		MoveAndSlide(vel.Normalized() * 25);
	}
	
	private void Die(){
		Node2D _smoke = (Node2D)smoke.Instance();
		_smoke.Position = Position;
		_smoke.Scale = new Vector2(2,2);
		GetTree().CurrentScene.AddChild(_smoke);
		QueueFree();
	}

	public void Hit(int i){
		health -=i;
		anim.Play("Hit");
		if(health<=0) Die();
	}
}
