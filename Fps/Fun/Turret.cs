using Godot;
using System;

public class Turret : Sprite{
	public PackedScene projactile = (PackedScene)ResourceLoader.Load("res://Projactile.tscn");
	public Position2D target;
	float fireRate = .2f;
	float time;
	float lastTimeFire;

	public override void _Ready() => target = GetNode<Position2D>("../Target");

	public override void _Process(float delta){
		time += delta;
		if(lastTimeFire + fireRate >= time) return;
		lastTimeFire = time;
		for(int i = 0; i < 1; i++){
			var bull = (Projactile)projactile.Instance();
			bull.Start(GlobalPosition, 0, target.GlobalPosition);
			GetTree().CurrentScene.AddChild(bull);
		}
	}
}
