using Godot;
using System;

public class Boss : Node2D
{
	public int health = 25;
	public Node2D player;
	public float time;
	public PackedScene projOne = (PackedScene)ResourceLoader.Load("res://Prefab/BossBulletOne.tscn");

	public override void _Ready(){
		player = GetNode<KinematicBody2D>(GetTree().CurrentScene.GetPath() + "/GamePlay/Player");
	}

	public override void _Process(float delta){
		time += delta;
	}

	public void Die(){
		GD.Print("Change StaGE");
	}
}

